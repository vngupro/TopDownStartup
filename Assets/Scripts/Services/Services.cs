using System;
using System.Runtime.CompilerServices;
using VContainer;
using VContainer.Unity;

internal class Services : LifetimeScope
{
    private const string AUTHORIZED_CALLER = "Awake";

    protected static IObjectResolver _container;

    public static T Resolve<T>([CallerMemberName] string caller = default)
    {
        if (caller != AUTHORIZED_CALLER)
            throw new InvalidOperationException($"Must Resolve through {AUTHORIZED_CALLER}");

        return _container.Resolve<T>();
    }

    protected override void Awake()
    {
        base.Awake();
        _container = Container;
    }
}