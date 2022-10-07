using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
public class GameMenager : MonoBehaviour
{
    [SerializeField] private CameraMovement _cameraMovement;
    [SerializeField] private GameObject _prefabWorm;
    [SerializeField] private int _teamAmount;
    [SerializeField] private int _teamSize;
    [SerializeField] public static List<List<GameObject>> Targets = new List<List<GameObject>>();
    public static int CurrentTeam;
    public static List<int> CurrentWormInTeam = new List<int>();
    public static GameObject CurrentWorm;
    public static GameObject PreviousWorm;
    [SerializeField] private float _checkRadiusForPlayers;
    [SerializeField] private float _checkRadiusForObstacles;
    public static List<List<PlayerMovement>> PlayerMovementTab = new List<List<PlayerMovement>>();
    public static List<List<PlayerWeaponScript>> PlayerWeaponTab = new List<List<PlayerWeaponScript>>();
    [SerializeField] private GameObject _mainCamera;
    private Color _randTeamColor;
    [SerializeField] private List<string> _adjective;
    [SerializeField] private List<string> _nameOrObject;
    [SerializeField] private float _timeForTurn;
    private float _currentTurnTime;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _StaminaText;
    void Start()
    {
        CurrentTeam = 0;
        for (int i = 0; i < _teamAmount; i++)
        {
            CurrentTeam = i + 1;
            CurrentWormInTeam.Add(0);
            Targets.Add(new List<GameObject>());
            PlayerMovementTab.Add(new List<PlayerMovement>());
            PlayerWeaponTab.Add(new List<PlayerWeaponScript>());
            _randTeamColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0f, 1f);
            for (int j = 0; j < _teamSize; j++)
            {
                Vector3 location = FindSpace();
                GameObject wormInstance = Instantiate(_prefabWorm, location, Quaternion.identity);
                PlayerLogic logicInstance = wormInstance.GetComponent<PlayerLogic>();
                PlayerMovement movementInstance = wormInstance.GetComponent<PlayerMovement>();
                PlayerWeaponScript WeaponScriptInstance = movementInstance.GetComponent<PlayerWeaponScript>();
                Targets[i].Add(wormInstance);
                Targets[i][j].transform.GetChild(1).GetComponent<Renderer>().material.color = _randTeamColor;
                PlayerMovementTab[i].Add(movementInstance);
                PlayerWeaponTab[i].Add(WeaponScriptInstance);
                movementInstance.GameMenager = gameObject.GetComponent<GameMenager>();
                WeaponScriptInstance.GameMenager = gameObject.GetComponent<GameMenager>();
               
                logicInstance.GameMenager = gameObject.GetComponent<GameMenager>();
                logicInstance.TeamColor = _randTeamColor;
                logicInstance.IndexTeam = i;
                logicInstance.IndexInTeam = j;
                logicInstance.name = _adjective[Random.Range(0, _adjective.Count)] + ' ' + _nameOrObject[Random.Range(0, _nameOrObject.Count)];
                movementInstance.StaminaText = _StaminaText;
                PlayerMovementTab[i][j].enabled = false;
                PlayerWeaponTab[i][j].enabled = false;

            }
        }
        CurrentTeam = 0;
        CurrentWorm = Targets[0][0];
        PlayerMovementTab[0][0].enabled = true;
        PlayerWeaponTab[0][0].enabled = true;
        SetCamera();
        _cameraMovement.UpdateTarget();
        SetTurnTime(_timeForTurn);
        // SetCurrentWorm();
    }
    void Update()
    {
        _currentTurnTime -= Time.deltaTime;
        _timerText.text = Mathf.Round(_currentTurnTime).ToShortString();
        if (_currentTurnTime <= 0)
        {
            _currentTurnTime = _timeForTurn;
            NextTurn();
        }

    }
    public Vector3 FindSpace()
    {
        int failsafe = 100;
        Vector3 temp = new Vector3(Random.Range(-19, 19), 0f, Random.Range(-19, 19));
        //Do a fail safe if it can't find a space to place a worm
        for (int i = 0; i < failsafe; i++)
        {
            if (IsFreeSpace(temp))
            {
                return temp;
            }
            temp = new Vector3(Random.Range(-19, 19), 0f, Random.Range(-19, 19));
        }
        // Throw the bad worms out coment it out for spawning them either way
        temp = new Vector3(-100f, 0f, -100f);   
        return temp;
    }

    public bool IsFreeSpace(Vector3 temp)
    {
        //Bigger space check for quality of life since you don't want the players to spawn right next to each other
        Collider[] tempColidersPlayers = Physics.OverlapSphere(temp, (_checkRadiusForPlayers));
        for (int j = 0; j < tempColidersPlayers.Length; j++)
        {
            if (tempColidersPlayers[j].CompareTag("Player"))
            {
                return false;
            }
        }
        Collider[] tempColidersObstacles = Physics.OverlapSphere(temp, (_checkRadiusForObstacles));
        for (int j = 0; j < tempColidersObstacles.Length; j++)
        {
            if (tempColidersObstacles[j].CompareTag("Obstacle"))
            {
                return false;
            }
        }
        return true;
    }
    public void NextTurn()
    {
        PreviousWorm = CurrentWorm;
        PlayerMovementTab[CurrentTeam][CurrentWormInTeam[CurrentTeam]].enabled = false;
        PlayerWeaponTab[CurrentTeam][CurrentWormInTeam[CurrentTeam]].enabled = false;

        _cameraMovement.SnpaBack();
        NextWormInTeam();
        NextTeam();

        _currentTurnTime = _timeForTurn;
        CurrentWorm = Targets[CurrentTeam][CurrentWormInTeam[CurrentTeam]];
        PlayerMovementTab[CurrentTeam][CurrentWormInTeam[CurrentTeam]].enabled = true;
        PlayerWeaponTab[CurrentTeam][CurrentWormInTeam[CurrentTeam]].enabled = true;
        SetCamera();
    }
    public void NextWormInTeam()
    {
        CurrentWormInTeam[CurrentTeam]++;
        if (CurrentWormInTeam[CurrentTeam] == Targets[CurrentTeam].Count)
        {
            CurrentWormInTeam[CurrentTeam] = 0;
        }
        CurrentWorm = Targets[CurrentTeam][CurrentWormInTeam[CurrentTeam]];
    }
    public void NextTeam()
    {
        CurrentTeam++;
        if (CurrentTeam == Targets.Count)
        {
            CurrentTeam = 0;
        }
        CurrentWorm = Targets[CurrentTeam][CurrentWormInTeam[CurrentTeam]];
    }
    public void SetCamera()
    {
        _mainCamera.transform.SetParent(CurrentWorm.transform, false);
        _cameraMovement.UpdateTarget();
    }

    public void PlayerDied(GameObject deadPlayer)
    {
        int index;
        for (int i = 0; i < Targets.Count; i++)
        {
            index = Targets[i].IndexOf(deadPlayer);
            if ((index != -1))
            {
                Targets[i].RemoveAt(index);
                PlayerMovementTab[i].RemoveAt(index);
                PlayerWeaponTab[i].RemoveAt(index);
                break;
            }

        }
    }
    public void Check(int teamIndex)
    {
        if (CurrentWormInTeam[teamIndex] > 0)
        {
            CurrentWormInTeam[teamIndex]--;
        }
        for (int i = 0; i < Targets.Count; i++)
        {
            if (Targets[i].Count == 0)
            {
                Targets.RemoveAt(i);
                CurrentTeam--;
                if (CurrentTeam < 0)
                {
                    CurrentTeam = 0;
                }
            }
        }
        if (Targets.Count <= 1)
        {
            EndGame();
        }
    }
    public void EndGame()
    {
        if (Targets.Count == 1)
        {
            Debug.LogError("GAME HAS ENDED TEAM " + (Targets[0][0].GetComponent<PlayerLogic>().IndexTeam + 1) + " WON!!!");
        }
        else
        {
            Debug.LogError("GAME ENDED AS A DRAW");
        }
    }
    public void SetTurnTime(float newTime)
    {
        _currentTurnTime = newTime;
    }
}