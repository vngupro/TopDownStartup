using VContainer;
using VContainer.Unity;

internal class Services : LifetimeScope
{
    protected static IObjectResolver _container;

    public static T Resolve<T>() => _container.Resolve<T>();

    protected override void Awake()
    {
        base.Awake();
        _container = Container;
    }
}