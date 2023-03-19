using UnityEngine;
using VContainer;
using VContainer.Unity;

internal class Services : LifetimeScope
{
    private static IObjectResolver _container;

    public static T Resolve<T>() => _container.Resolve<T>();

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