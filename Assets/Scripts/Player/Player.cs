using UnityEngine;
using Utils;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(HealthModule))]
[RequireComponent(typeof(ShootModule))]
internal sealed class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [Space]
    [SerializeField] private HealthModule _healthModule;
    [SerializeField] private ShootModule _shootModule;
    [Space]
    [SerializeField] private float _speed = 10;

    private Vector2 _input;

    private void Update()
    {
        _input = new Vector2(
            Input.GetAxisRaw(InputUtils.AXIS_HORIZONTAL),
            Input.GetAxisRaw(InputUtils.AXIS_VERTICAL)
        );
    }

    private void FixedUpdate() => _rb.velocity = _speed * _input;

    private void Reset()
    {
        _rb = GetComponent<Rigidbody>();
        _healthModule = GetComponent<HealthModule>();
        _shootModule = GetComponent<ShootModule>();
    }
}