using UnityEngine;

internal sealed class HeyService : MonoBehaviour, IHeyService
{
    void IHeyService.Hey() => Debug.Log("Hey");
}