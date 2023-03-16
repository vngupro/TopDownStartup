using UnityEngine;
using Utils;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(HealthModule))]
public sealed class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [Space]
    [SerializeField] private HealthModule _healthModule;
    [Space]
    [SerializeField] private float _speed = 1;

    private void Update()
    {
        var hInput = Input.GetAxis(InputUtils.AXIS_HORIZONTAL);
        var vInput = Input.GetAxis(InputUtils.AXIS_VERTICAL);

        _rb.velocity = new Vector2(hInput, vInput);
    }

    private void Reset()
    {
        _rb = GetComponent<Rigidbody>();
        _healthModule = GetComponent<HealthModule>();
    }
}