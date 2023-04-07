using UnityEngine;

internal sealed class Projectile : Poolable
{
    [SerializeField]
    private float speed = 5;
    [SerializeField]
    private float damage = 50;
    private Vector3 direction;
    private void Start() => Release();

    public void Shoot(Transform initial)
    {
        transform.position = initial.position;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = (mousePos - initial.position).normalized;
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        Enemy e;
        if(c.gameObject.TryGetComponent<Enemy>(out e)) 
        {
            e.gameObject.GetComponent<HealthModule>().ApplyDamage(damage);
        }
    }
}