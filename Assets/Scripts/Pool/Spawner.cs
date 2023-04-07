using UnityEngine;
using UnityEngine.Pool;

internal sealed class Spawner : MonoBehaviour
{
    public IObjectPool<Poolable> Pool { get; private set; }

    [field: SerializeField] public Poolable Prefab { get; set; }

    [SerializeField] private int _defaultCapacity = 10;
    [SerializeField] private int _maxPoolSize = 20;

    private GameObject _container;

    private void Awake()
    {
        Pool = new ObjectPool<Poolable>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, _defaultCapacity, _maxPoolSize);
    }

    private void Start()
    {
        _container = new($"{Prefab.name}Pool");
    }

    public Poolable Spawn()
    {
        var obj = Pool.Get();
        obj.transform.SetParent(_container.transform);
        return obj;
    }

    private Poolable CreatePooledItem()
    {
        var obj = Instantiate(Prefab);
        obj.Pool = Pool;
        return obj;
    }

    private void OnTakeFromPool(Poolable obj) => obj.gameObject.SetActive(true);

    private void OnReturnedToPool(Poolable obj) => obj.gameObject.SetActive(false);

    private void OnDestroyPoolObject(Poolable obj) => Destroy(obj);
}