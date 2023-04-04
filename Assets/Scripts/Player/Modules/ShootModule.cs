using UnityEngine;
using Utils;

// WIP
internal sealed class ShootModule : MonoBehaviour
{
    private const string BULLETS_CONTAINER = "BulletsContainer";

    [SerializeField] private GameObject _bullet;

    private void Awake()
    {
        Instantiate(new GameObject(BULLETS_CONTAINER), transform);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown((int)InputUtils.MouseButton.Left))
            Shoot();
    }

    private void Shoot()
    {
        Instantiate(_bullet, transform.localPosition, Quaternion.identity);
    }
}