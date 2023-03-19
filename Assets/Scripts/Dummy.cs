using UnityEngine;

internal sealed class Dummy : MonoBehaviour
{
    private static IYoService _yoService;

    private void Awake() => _yoService ??= Services.Resolve<IYoService>();

    private void Start() => _yoService.Yo();
}