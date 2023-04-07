using UnityEngine;
using Utils;

internal sealed class ShootModule : MonoBehaviour
{
    private static IAudioService _audioService;

    [SerializeField] private Poolable _projectilePrefab;

    private AudioClip _shootSFX;

    private Spawner _projectileSpawner;

    private void Awake()
    {
        _audioService ??= Services.Resolve<IAudioService>();
    }

    private void Start()
    {
        _shootSFX = Resources.Load("AudioResources/SFX/ShootSFX") as AudioClip;

        _projectileSpawner = new GameObject(nameof(Spawner)).AddComponent<Spawner>();
        _projectileSpawner.Prefab = _projectilePrefab;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown((int)InputUtils.MouseButton.Left))
            Shoot();
    }

    private void Shoot()
    {
        _audioService.PlaySound(AUDIO_CHANNEL.SFX, _shootSFX);

        _projectileSpawner.Spawn();
    }
}