using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class CharacterProjectile : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform spawnPoint;
    [SerializeField] float bulletSpeed;
    [SerializeField] Transform crosshair;
    [SerializeField] float distance;


    bool isClick;
    public Camera mainCamera;
    private float distanceFromCamera;
    private void Start()
    {
        distanceFromCamera = Vector3.Distance(crosshair.position, mainCamera.transform.position);
    }

    private void Update()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        crosshair.position = mainCamera.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, distanceFromCamera + distance));
        Vector3 targetPosition = mainCamera.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, distanceFromCamera + distance));
        isClick = Input.GetMouseButtonDown(0);

       
        if (isClick)
        {
            Fire();
        }
    }

    void Fire()
    {
        var bullet = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
        var rb = bullet.AddComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        Vector3 fireDirection = (crosshair.position - spawnPoint.position).normalized;
        rb.AddForce(fireDirection * bulletSpeed, ForceMode.Impulse);

        Destroy(bullet, 3f); // 3 saniye sonra mermiyi yok etmek için
    }

}
