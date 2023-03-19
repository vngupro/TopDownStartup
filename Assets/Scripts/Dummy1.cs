using UnityEngine;

internal sealed class Dummy1 : MonoBehaviour
{
    private static IHeyService _heyService;

    private void Awake() => _heyService ??= Services.Resolve<IHeyService>();

    private void Start() => _heyService.Hey();
}