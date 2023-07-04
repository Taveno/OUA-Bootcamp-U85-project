using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class CharacterProjectile : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform spawnPoint;

    [SerializeField] float bulletSpeed;


    bool isClick;


    void Start()
    {

    }

    private void Update()
    {
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
        rb.AddForce(spawnPoint.forward * bulletSpeed, ForceMode.Impulse);
        Destroy(bullet, 3f); // 3 saniye sonra mermiyi yok etmek için
    }
}
