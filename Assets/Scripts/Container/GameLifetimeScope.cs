using UnityEngine;
using VContainer;
using VContainer.Unity;

internal class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private HelloScreen _helloScreen;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<GamePresenter>();

        builder.Register<IHelloWorldService, HelloWorldService>(Lifetime.Singleton);

        builder.RegisterComponent(_helloScreen);
    }
}