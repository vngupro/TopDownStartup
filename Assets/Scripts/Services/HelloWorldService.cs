using UnityEngine;

internal interface IHelloWorldService
{
    void Hello();
}

internal sealed class HelloWorldService : IHelloWorldService
{
    void IHelloWorldService.Hello() => Debug.Log("Hello");
}

internal sealed class HelloWorldErrorService : IHelloWorldService
{
    void IHelloWorldService.Hello() => Debug.LogError("Hello");
}