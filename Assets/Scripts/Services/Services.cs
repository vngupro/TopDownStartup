using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using VContainer;
using VContainer.Unity;

internal class Services : LifetimeScope
{
    private const string AUTHORIZED_CALLER = "Awake";

    private static IObjectResolver _container;

    public static T Resolve<T>([CallerMemberName] string caller = default)
    {
        if (caller != AUTHORIZED_CALLER)
            throw new InvalidOperationException($"Must Resolve through {AUTHORIZED_CALLER}");

        return _container.Resolve<T>();
    }

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterBuildCallback(container => _container = container);

        builder.UseComponents(transform, components =>
        {
            components.AddOnNewGameObject<YoService>(Lifetime.Singleton, nameof(YoService)).As<IYoService>();
        });
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize() => DontDestroyOnLoad(new GameObject(nameof(Services), typeof(Services)));
}