using VContainer;
using VContainer.Unity;

internal class SceneSpecificServices : Services
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.UseComponents(transform, components =>
        {
            components.AddOnNewGameObject<HeyService>(Lifetime.Singleton, nameof(HeyService)).As<IHeyService>();
        });
    }
}