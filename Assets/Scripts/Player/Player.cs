using System;
using TMPro.EditorUtilities;
using UnityEngine;
using Utils;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(HealthModule))]
[RequireComponent(typeof(ShootModule))]
internal sealed class Player : MonoBehaviour
{
    private static IPlayerService _playerService;

    [SerializeField] private Rigidbody2D _rb;
    [Space]
    [SerializeField] private HealthModule _healthModule;
    [SerializeField] private ShootModule _shootModule;
    [Space]
    [SerializeField] private float _speed = 10;

    private Vector2 _input;

    private BSP_Master _bsp;

    /*private void Awake() => _playerService ??= Services.Resolve<IPlayerService>();

    private void Start()
    {
        _playerService.AddPlayer(this);
    }*/
    
    private void Start()
    {
        _bsp = FindObjectOfType<BSP_Master>();
        _bsp.OnDungeonFinishGenerate += SetToSpawnPoint;
    }

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
        _rb = GetComponent<Rigidbody2D>();
        _healthModule = GetComponent<HealthModule>();
        _shootModule = GetComponent<ShootModule>();
    }

    private void OnDestroy() => _bsp.OnDungeonFinishGenerate -= SetToSpawnPoint;
    private void SetToSpawnPoint() => transform.position = _bsp.SpawnPoint;

    private void OnTriggerEnter2D(Collider2D col)
    {
        col.GetComponent<Stairs>()?.ClimbStairs();
    }
}