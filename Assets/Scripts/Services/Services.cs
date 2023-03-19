using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

internal static class Services
{
    /// <summary>
    /// Proceed services registration
    /// </summary>
    private static void Initialize()
    {
        Register<IYoService, YoService>();
    }

    private static readonly GameObject _root = new(nameof(Services));

    private static readonly Dictionary<string, MonoBehaviour> _services = new();

    private static void Register<Abstr, Impl>() where Impl : MonoBehaviour
    {
        var key = typeof(Abstr).Name;

        if (_services.ContainsKey(key))
            throw new InvalidOperationException($"{key} is already registered");

        var value = new GameObject(typeof(Impl).Name).AddComponent<Impl>();
        value.transform.SetParent(_root.transform);

        _services.Add(key, value);
    }

    public static Abstr Resolve<Abstr>() where Abstr : class
    {
        var key = typeof(Abstr).Name;

        if (!_services.TryGetValue(key, out var value))
            throw new InvalidOperationException($"{key} is not registered");

        return value as Abstr;
    }

    static Services()
    {
        Object.DontDestroyOnLoad(_root);

        Initialize();
    }
}