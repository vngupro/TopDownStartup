using VContainer;
using VContainer.Unity;

internal class Services : LifetimeScope
{
    protected static Services _services;

    public static T Resolve<T>() => _services.Container.Resolve<T>();

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        _services = this;
    }
}