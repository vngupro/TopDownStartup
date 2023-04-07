using UnityEngine;
using Utils;

// WIP
internal sealed class ShootModule : MonoBehaviour
{
    private const string BULLETS_CONTAINER = "BulletsContainer";
    private static IAudioService _audioService;

    private AudioClip _shootSFX;
    [SerializeField] private GameObject _bullet;

    private void Awake()
    {
        _audioService ??= Services.Resolve<IAudioService>();
        Instantiate(new GameObject(BULLETS_CONTAINER), transform);
    }

    private void Start()
    {
        _shootSFX = Resources.Load("AudioResources/SFX/ShootSFX") as AudioClip;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown((int)InputUtils.MouseButton.Left))
            Shoot();
    }

    private void Shoot()
    {
        Instantiate(_bullet, transform.localPosition, Quaternion.identity);
        _audioService.PlaySound(AUDIO_CHANNEL.SFX, _shootSFX);
    }
}