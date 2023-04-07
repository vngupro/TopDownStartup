using UnityEngine;
using UnityEngine.Pool;

internal class Poolable : MonoBehaviour
{
    public IObjectPool<Poolable> Pool { get; set; }

    public void Release() => Pool.Release(this);
}