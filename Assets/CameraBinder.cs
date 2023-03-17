using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBinder : MonoBehaviour
{

    [SerializeField] Cinemachine.CinemachineVirtualCamera _cam;
    [SerializeField] PlayerReference _playerRef;

    void Start()
    {
        UpdateCameraFollow(_playerRef.Instance);

        _playerRef.OnValueChanged += UpdateCameraFollow;
    }

    private void OnDestroy()
    {
        _playerRef.OnValueChanged -= UpdateCameraFollow;
    }

    private void UpdateCameraFollow(Entity obj)
    {
        _cam.Follow = obj.transform;
    }
}
