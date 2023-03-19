using UnityEngine;
using VContainer;
using VContainer.Unity;

internal class CommonServices : Services
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.UseComponents(transform, components =>
        {
            components.AddOnNewGameObject<YoService>(Lifetime.Singleton, nameof(YoService)).As<IYoService>();
        });
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize() => DontDestroyOnLoad(new GameObject(nameof(CommonServices), typeof(CommonServices)));
}