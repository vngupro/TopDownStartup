using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAnimator : MonoBehaviour
{
    [SerializeField, Required, BoxGroup("Dependencies")] Animator _animator;
    [SerializeField, Required, BoxGroup("Dependencies")] EntityMovement _movement;

    [BoxGroup("Animator Param")]
    [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Bool)]
    int _isWalkingParam;

    #region Editor
#if UNITY_EDITOR
    void Reset()
    {
        _animator = GetComponent<Animator>() ?? GetComponentInChildren<Animator>();
        _movement = GetComponentInParent<Entity>().GetComponentInChildren<EntityMovement>();
    }
#endif
    #endregion

    void Start()
    {
        _movement.OnStartWalking += MoveStart;
        _movement.OnStopWalking += MoveStop;
    }
    void OnDestroy()
    {
        _movement.OnStartWalking -= MoveStart;
        _movement.OnStopWalking -= MoveStop;
    }

    void MoveStart() => _animator.SetBool(_isWalkingParam, true);
    void MoveStop() => _animator.SetBool(_isWalkingParam, false);

}
