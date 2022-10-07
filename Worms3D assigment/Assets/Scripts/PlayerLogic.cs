using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class PlayerLogic : MonoBehaviour, IKillable
{
    public GameMenager GameMenager;
    [SerializeField] private int _maxHp;
    private int _hp;
    [SerializeField] private float _speed;
    public string NamePlayer;
    public string NameTeam;
    public int IndexTeam;
    public int IndexInTeam;
    public Color TeamColor;
    [SerializeField] private Transform _canvas;
    private TextMeshProUGUI _hpDisplay;
    private TextMeshProUGUI _namePlayerDisplay;
    private TextMeshProUGUI _nameTeamDisplay;
    public List<GameObject> WeaponList;
    public GameObject CurrentWeapon;
    public int WeaponIndex = 0;
    void Start()
    {
        _canvas = gameObject.transform.GetChild(0);
        //somehow get team number 
        NameTeam = "team " + (IndexTeam + 1);
        _nameTeamDisplay = _canvas.GetChild(0).GetComponent<TextMeshProUGUI>();
        _nameTeamDisplay.text = NameTeam;
        _nameTeamDisplay.color = TeamColor;
        NamePlayer = this.name;
        _namePlayerDisplay = _canvas.GetChild(1).GetComponent<TextMeshProUGUI>();
        _namePlayerDisplay.text = NamePlayer;
        _namePlayerDisplay.color = TeamColor;
        _hp = _maxHp;
        _hpDisplay = _canvas.GetChild(2).GetComponent<TextMeshProUGUI>();
        _hpDisplay.text = _hp.ToString() + " HP";
        _canvas.GetChild(2).GetComponent<TextMeshProUGUI>().color = TeamColor;
        gameObject.GetComponent<PlayerMovement>().Speed = _speed;
    }
    void Update()
    {
        _canvas.rotation = Camera.main.transform.rotation;
    }
    public void TakeDamage(int amount)
    {
        _hp -= amount;
        if (_hp <= 0)
        {
            Death();
        }
        _hpDisplay.text = _hp.ToString() + " HP";
    }
    public void Death()
    {
        if (GameMenager.CurrentWorm==gameObject)
        {
            GameMenager.NextTurn();
        }
        GameMenager.PlayerDied(gameObject);
        GameMenager.Check(IndexTeam);
        Destroy(gameObject);
    }
}
