using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public GameMenager GameMenager;
    public WormControls _wormControls;
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private PlayerLogic _playerLogic;
    [SerializeField] public float Speed;
    [SerializeField] private float _maxStamina;
    [SerializeField] private float _currentStamina;
    public TextMeshProUGUI StaminaText;
    private void Awake()
    {
        _wormControls = new WormControls();
    }
    private void OnEnable()
    {
        EnableInput();
        _currentStamina = _maxStamina;
    }
    private void OnDisable()
    {
        DisableInput();
    }
    private void FixedUpdate()
    {
        float moveFloat = _wormControls.Player.Move.ReadValue<float>();
        float rotateFloat = _wormControls.Player.Rotate.ReadValue<float>();
        _currentStamina -= moveFloat * Time.deltaTime;
        StaminaText.text = _currentStamina.ToString();
        if (_currentStamina <= 0)
        {
            GameMenager.NextTurn();
        }
        _rigidBody.velocity = (transform.forward * moveFloat) * Speed * Time.deltaTime;
        transform.Rotate(0f, rotateFloat, 0f, Space.World);
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 mouseDelta = context.ReadValue<Vector2>();
        transform.Rotate(new Vector3(0f, 1f, 0f), mouseDelta.x * 0.1f);
    }
    public void Move(InputAction.CallbackContext context)
    {

    }
    public void Rotate(InputAction.CallbackContext context)
    {
        
    }
    public void Aim(InputAction.CallbackContext context)
    {
       
    }
    public void EnableInput()
    {
        EnablePlayerMovement();
        EnablePlayerRotate();
    }
    public void DisableInput()
    {
        DisablePlayerMovement();
        DisablePlayerRotate();
    }
    public void EnablePlayerMovement()
    {
        _wormControls.Player.Move.Enable();
        _wormControls.Player.Move.performed += Move;
    }
    public void EnablePlayerRotate()
    {
        _wormControls.Player.Rotate.Enable();
        _wormControls.Player.Rotate.performed += Rotate;
    }
    public void EnablePlayerAim()
    {
        _wormControls.Player.Aim.Enable();
        _wormControls.Player.Aim.performed += Aim;
    }
    public void DisablePlayerMovement()
    {
        _wormControls.Player.Move.Disable();
        _wormControls.Player.Move.performed -= Move;
    }
    public void DisablePlayerRotate()
    {
        _wormControls.Player.Rotate.Disable();
        _wormControls.Player.Rotate.performed -= Rotate;
    }
    public void DisablePlayerAim()
    {
        _wormControls.Player.Aim.Disable();
        _wormControls.Player.Aim.performed -= Aim;
    }
}
