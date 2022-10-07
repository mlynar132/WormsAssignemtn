using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Vector3 _offsetPos;
    [SerializeField] private float _cameraMoveSpeed;
    [SerializeField] private float _sprintMulti;
    public WormControls WormControls;
    private GameObject _currentWorm;
    private float _yaw = 0f;
    private float _pitch = 0f;
    private void Awake()
    {
        WormControls = new WormControls();
    }

    private void OnEnable()
    {
        WormControls.Camera.LookPlayer.Enable();
        WormControls.Camera.Dislocate.Enable();
        WormControls.Camera.LookPlayer.performed += LookPlayer;
        WormControls.Camera.Dislocate.performed += DislocateButton;
    }

    private void OnDisable()
    {
        WormControls.Camera.Disable();
        WormControls.Camera.LookPlayer.performed -= LookPlayer;
        WormControls.Camera.Move.performed -= Move;
        WormControls.Camera.Sprint.performed -= Sprint;
        WormControls.Camera.Snap.performed -= SnapBackButton;
        WormControls.Camera.Dislocate.performed -= DislocateButton;
    }
    void LateUpdate()
    {
        Vector2 movement = WormControls.Camera.Move.ReadValue<Vector2>();
        transform.Translate(new Vector3(movement.x, 0f, movement.y).normalized * _cameraMoveSpeed + (WormControls.Camera.Sprint.ReadValue<float>() * _sprintMulti * new Vector3(movement.x, 0f, movement.y).normalized));
    }
    public void Move(InputAction.CallbackContext context)
    {

    }
    public void Sprint(InputAction.CallbackContext context)
    {

    }
    public void LookPlayer(InputAction.CallbackContext context)
    {
        Vector2 mouseDelta = context.ReadValue<Vector2>();
        transform.RotateAround(_currentWorm.transform.position, transform.up, mouseDelta.x * 0.1f);
    }
    public void LookFree(InputAction.CallbackContext context)
    {
        Vector2 mouseDelta = context.ReadValue<Vector2>();
        _yaw += mouseDelta.x * 0.1f;
        _pitch -= mouseDelta.y * 0.1f;
        transform.eulerAngles = new Vector3(_pitch, _yaw, 0f);
    }
    private void SnapBackButton(InputAction.CallbackContext context)
    {
        SnpaBack();
    }

    private void DislocateButton(InputAction.CallbackContext context)
    {
        Dislocate();
    }
    public void UpdateTarget()
    {
        _currentWorm = GameMenager.CurrentWorm;
    }
    public void SnpaBack()
    {
        gameObject.transform.localPosition = _offsetPos;
        gameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);

        WormControls.Camera.Move.Disable();
        WormControls.Camera.Sprint.Disable();
        WormControls.Camera.Snap.Disable();
        WormControls.Camera.LookFree.Disable();
        WormControls.Camera.Move.performed -= Move;
        WormControls.Camera.Sprint.performed -= Sprint;
        WormControls.Camera.Snap.performed -= SnapBackButton;
        WormControls.Camera.LookFree.performed -= LookFree;

        _currentWorm.GetComponent<PlayerMovement>().EnableInput();

        WormControls.Camera.Dislocate.Enable();
        WormControls.Camera.LookPlayer.Enable();
        WormControls.Camera.Dislocate.performed += DislocateButton;
        WormControls.Camera.LookPlayer.performed += LookPlayer;
    }
    private void Dislocate()
    {
        _yaw = transform.rotation.eulerAngles.y;
        _pitch = transform.rotation.eulerAngles.x;

        WormControls.Camera.Move.Enable();
        WormControls.Camera.Sprint.Enable();
        WormControls.Camera.Snap.Enable();
        WormControls.Camera.LookFree.Enable();
        WormControls.Camera.Move.performed += Move;
        WormControls.Camera.Sprint.performed += Sprint;
        WormControls.Camera.Snap.performed += SnapBackButton;
        WormControls.Camera.LookFree.performed += LookFree;

        _currentWorm.GetComponent<PlayerMovement>().DisableInput();


        WormControls.Camera.Dislocate.Disable();
        WormControls.Camera.LookPlayer.Disable();
        WormControls.Camera.Dislocate.performed -= DislocateButton;
        WormControls.Camera.LookPlayer.performed -= LookPlayer;
    }
}
