using VContainer;
using VContainer.Unity;

internal sealed class GamePresenter : IStartable
{
    [Inject] private readonly IHelloWorldService _helloWorldService;

    [Inject] private readonly HelloScreen _helloScreen;

    void IStartable.Start() => _helloScreen.HelloButton.onClick.AddListener(_helloWorldService.Hello);
}