using System.Collections;
using System.Collections.Generic;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponScript : MonoBehaviour
{
    [SerializeField] private PlayerLogic _playerLogic;
    [SerializeField] private PlayerMovement _playerMovement;
    public GameMenager GameMenager;
    private List<GameObject> _weaponList;
    private int _weaponIndex;
    [SerializeField] private GameObject _currentWeapon;
    [SerializeField] private GameObject _weaponInstance;
    private WormControls _wormControls;
    private IWeapon _interfaceOfWeapon;
    [SerializeField] private float _chargeTime;
    private bool _isCharging;
    [SerializeField] private float _chargingSpeed;
    [SerializeField] private float _timeAfterAction;
    private bool _skiped = false;
    private bool _shot = false;
    private void Start()
    {
        _weaponIndex = _playerLogic.WeaponIndex;
        _weaponList = _playerLogic.WeaponList;
        SetWeapon();
        _skiped = true;
    }
    private void Awake()
    {
        _wormControls = new WormControls();
    }
    private void OnEnable()
    {
        _wormControls.Player.Fire.Enable();
        _wormControls.Player.PreviousWeapon.Enable();
        _wormControls.Player.NextWeapon.Enable();
        _wormControls.Player.Fire.performed += Fire;
        _wormControls.Player.Fire.canceled += Fire;
        _wormControls.Player.PreviousWeapon.performed += PreviousWeaponButton;
        _wormControls.Player.NextWeapon.performed += NextWeaponButton;
        if (_skiped)
        {
            SetWeapon();
        }
        _shot = false;
    }
    private void OnDisable()
    {
        _wormControls.Player.Fire.Disable();
        _wormControls.Player.PreviousWeapon.Disable();
        _wormControls.Player.NextWeapon.Disable();
        _wormControls.Player.Fire.performed -= Fire;
        _wormControls.Player.PreviousWeapon.performed -= PreviousWeaponButton;
        _wormControls.Player.NextWeapon.performed -= NextWeaponButton;
        Destroy(_weaponInstance);
    }
    private void FixedUpdate()
    {
        float aimFloat = _playerMovement._wormControls.Player.Aim.ReadValue<float>();
        if (_interfaceOfWeapon != null && _isCharging)
        {
            _interfaceOfWeapon.Aim(aimFloat);
        }
    }
    private void Update()
    {
        if (_isCharging)
        {
            _chargeTime += Time.deltaTime * _chargingSpeed;
            _chargeTime = Mathf.Clamp(_chargeTime, 0f, 1f);
        }
    }
    public void PreviousWeaponButton(InputAction.CallbackContext context)
    {
        PreviousWeapon();
    }
    public void NextWeaponButton(InputAction.CallbackContext context)
    {
        NextWeapon();
    }
    public void Fire(InputAction.CallbackContext context)
    {
        if (!_shot)
        {
            if (context.performed)
            {
                SwitchToShootingState();
                weaponCharger();
            }
            if (context.canceled)
            {
                FireWeapon();
                _isCharging = false;
                SwitchToMovingState();
                BlockWeaponSwitch();
                GameMenager.SetTurnTime(_timeAfterAction);
                _shot = true;
            }
        }
    }
    public void NextWeapon()
    {
        _weaponIndex += 1;
        if (_weaponIndex >= _weaponList.Count)
        {
            _weaponIndex = 0;
        }
        SetWeapon();
    }
    public void PreviousWeapon()
    {
        _weaponIndex -= 1;
        if (_weaponIndex < 0)
        {
            _weaponIndex = _weaponList.Count - 1;
        }
        SetWeapon();
    }
    public void SetWeapon()
    {
        Destroy(_weaponInstance);
        _currentWeapon = _weaponList[_weaponIndex];
        _weaponInstance = Instantiate(_currentWeapon, gameObject.transform);
        _weaponInstance.transform.localRotation = _currentWeapon.transform.rotation;
        _weaponInstance.transform.localPosition = _currentWeapon.transform.position;
        _interfaceOfWeapon = _weaponInstance.GetComponent<IWeapon>();
    }
    void SwitchToShootingState()
    {
        BlockWeaponSwitch();
        _playerMovement.DisablePlayerMovement();
        _playerMovement.EnablePlayerAim();
    }
    void SwitchToMovingState()
    {
        UnlockWeaponSwitch();
        _playerMovement.EnablePlayerMovement();
        _playerMovement.DisablePlayerAim();
    }
    void BlockWeaponSwitch()
    {
        _wormControls.Player.PreviousWeapon.Disable();
        _wormControls.Player.NextWeapon.Disable();
        _wormControls.Player.PreviousWeapon.performed -= PreviousWeaponButton;
        _wormControls.Player.NextWeapon.performed -= NextWeaponButton;
    }
    void UnlockWeaponSwitch()
    {
        _wormControls.Player.PreviousWeapon.Enable();
        _wormControls.Player.NextWeapon.Enable();
        _wormControls.Player.PreviousWeapon.performed += PreviousWeaponButton;
        _wormControls.Player.NextWeapon.performed += NextWeaponButton;
    }
    public void FireWeapon()
    {
        if (_interfaceOfWeapon != null)
        {
            _interfaceOfWeapon.Shoot(_chargeTime);
        }
    }
    void weaponCharger()
    {
        _chargeTime = 0;
        _isCharging = true;
    }
}