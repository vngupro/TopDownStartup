using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBrain : MonoBehaviour
{
    [SerializeField, BoxGroup("Dependencies")] EntityMovement _movement;

    [SerializeField, BoxGroup("Input")] InputActionProperty _moveInput;

    private void Start()
    {
        _moveInput.action.started += UpdateMove;
        _moveInput.action.performed += UpdateMove;
        _moveInput.action.canceled += StopMove;
    }

    private void OnDestroy()
    {
        _moveInput.action.started -= UpdateMove;
        _moveInput.action.performed -= UpdateMove;
        _moveInput.action.canceled -= StopMove;
    }

    private void UpdateMove(InputAction.CallbackContext obj)
    {
        _movement.Move(obj.ReadValue<Vector2>().normalized);
    }
    private void StopMove(InputAction.CallbackContext obj)
    {
        _movement.Move(Vector2.zero);
    }
}
