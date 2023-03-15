using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBrain : MonoBehaviour
{
    [SerializeField, BoxGroup("Dependencies")] EntityMovement _movement;
    [SerializeField, BoxGroup("Dependencies")] EntityAttack _attack;

    [SerializeField, BoxGroup("Input")] InputActionProperty _moveInput;
    [SerializeField, BoxGroup("Input")] InputActionProperty _attackInput;

    private void Start()
    {
        // Move
        _moveInput.action.started += UpdateMove;
        _moveInput.action.performed += UpdateMove;
        _moveInput.action.canceled += StopMove;
        // Attack
        _attackInput.action.started += Attack;
    }


    private void OnDestroy()
    {
        // Move
        _moveInput.action.started -= UpdateMove;
        _moveInput.action.performed -= UpdateMove;
        _moveInput.action.canceled -= StopMove;
        // Attack
        _attackInput.action.started += Attack;
    }

    private void Attack(InputAction.CallbackContext obj) => _attack.LaunchAttack();

    private void UpdateMove(InputAction.CallbackContext obj)
    {
        _movement.Move(obj.ReadValue<Vector2>().normalized);
    }
    private void StopMove(InputAction.CallbackContext obj)
    {
        _movement.Move(Vector2.zero);
    }
}
