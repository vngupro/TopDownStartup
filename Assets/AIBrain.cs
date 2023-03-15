using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBrain : MonoBehaviour
{
    [SerializeField, BoxGroup("Special Dependency")] Entity _playerEntity;

    [SerializeField, BoxGroup("Dependencies")] Entity _root;
    [SerializeField, BoxGroup("Dependencies")] EntityMovement _movement;

    [SerializeField, BoxGroup("Conf")] float _distanceDetection;
    [SerializeField, BoxGroup("Conf")] float _stopDistance;

    bool IsPlayerNear => Vector3.Distance(_root.transform.position, _playerEntity.transform.position) < _distanceDetection;
    bool IsPlayerTooNear => Vector3.Distance(_root.transform.position, _playerEntity.transform.position) < _stopDistance;

    #region EDITOR
#if UNITY_EDITOR
    void Reset()
    {
        _playerEntity = null;
        _root = GetComponentInParent<Entity>();
        _movement = _root.GetComponentInChildren<EntityMovement>();
        _distanceDetection = 3f;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_root.transform.position, _distanceDetection);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_root.transform.position, _stopDistance);
    }
#endif
    #endregion

    private void Update()
    {
        // Attack
        if(IsPlayerTooNear)
        {
            _movement.Move(Vector2.zero);
            // Attack
        }
        // Move To Player
        else if (IsPlayerNear)
        {
            _movement.MoveToward(_playerEntity.transform);
        }
        // Stay idle
        else
        {
            _movement.Move(Vector2.zero);
        }

    }


}
