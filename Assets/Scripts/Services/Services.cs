using UnityEngine;
using VContainer;
using VContainer.Unity;

internal class Services : LifetimeScope
{
    private static Services _services;

    public static T Resolve<T>() => _services.Container.Resolve<T>();

    protected override void Configure(IContainerBuilder builder)
    {
        _services = this;

        builder.UseComponents(transform, components =>
        {
            components.AddOnNewGameObject<YoService>(Lifetime.Singleton, nameof(YoService)).As<IYoService>();
        });
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize() => DontDestroyOnLoad(new GameObject(nameof(Services), typeof(Services)));
}