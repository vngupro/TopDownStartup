internal sealed class Projectile : Poolable
{
    private void Start() => Release();
}