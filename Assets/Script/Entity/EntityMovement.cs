using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityMovement : MonoBehaviour
{
    [SerializeField, BoxGroup("Dependencies")] Rigidbody2D _rb;

    [SerializeField, BoxGroup("Configuration")] float _speed;

    [SerializeField, Foldout("Event")] UnityEvent _onStartWalking;
    [SerializeField, Foldout("Event")] UnityEvent _onContinueWalking;
    [SerializeField, Foldout("Event")] UnityEvent _onStopWalking;

    public event UnityAction OnStartWalking { add => _onStartWalking.AddListener(value); remove=> _onStartWalking.RemoveListener(value); }
    public event UnityAction OnContinueWalking { add => _onContinueWalking.AddListener(value); remove=> _onContinueWalking.RemoveListener(value); }
    public event UnityAction OnStopWalking { add => _onStopWalking.AddListener(value); remove=> _onStopWalking.RemoveListener(value); }

    Vector2 MoveDirection { get; set; }
    Vector2 OldVelocity { get; set; }

    #region EDITOR
#if UNITY_EDITOR
    private void Reset()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
        _speed = 1f;
    }
#endif
    #endregion

    public void Move(Vector2 direction) => MoveDirection = direction.normalized;
    public void MoveToward(Transform target) => MoveDirection = (target.position - _rb.transform.position).normalized;

    private void FixedUpdate()
    {
        // FireEvents
        if (MoveDirection.magnitude < 0.01f && OldVelocity.magnitude > 0.01f)
            _onStopWalking?.Invoke();
        else if (MoveDirection.magnitude > 0.01f && OldVelocity.magnitude < 0.01f)
            _onStartWalking?.Invoke();
        else _onContinueWalking?.Invoke();

        // Physics
        _rb.AddForce(MoveDirection * _speed * Time.fixedDeltaTime, ForceMode2D.Force);

        // Keep old data
        OldVelocity = _rb.velocity;
    }

}
