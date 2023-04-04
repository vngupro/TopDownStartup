using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FollowMousePosition : MonoBehaviour
{
    [SerializeField] InputActionReference _mousePosition;
    [SerializeField] Camera _referenceCamera;

    void Update()
    {
        var p = _referenceCamera.ScreenToWorldPoint(_mousePosition.action.ReadValue<Vector2>());
        transform.position = new Vector3(p.x, p.y, 0);
    }



}
