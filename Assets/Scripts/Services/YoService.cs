using UnityEngine;

internal sealed class YoService : MonoBehaviour, IYoService
{
    void IYoService.Yo() => Debug.Log("Yo");
}

internal sealed class YoWarningService : MonoBehaviour, IYoService
{
    void IYoService.Yo() => Debug.LogWarning("Yo");
}

internal sealed class YoErrorService : MonoBehaviour, IYoService
{
    void IYoService.Yo() => Debug.LogError("Yo");
}