using UnityEngine;

internal sealed class Dummy : MonoBehaviour
{
    private static IYoService _yoService;

    private void Awake() => _yoService ??= Services.Resolve<IYoService>();

    private void Start() => _yoService.Yo();

    private void OnEnable() => new Dummy1();
}

internal sealed class Dummy1
{
    public Dummy1() => Services.Resolve<IYoService>().Yo();
}