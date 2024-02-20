using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{

    public SpreadBullet bullet;

    public Vector2 direction;

    public Vector2 centerLocation;

    [SerializeField]
    private float fireForce = 1f;

    private void Update()
    {
        direction = (transform.localRotation * -centerLocation).normalized;
    }

    public void Shoot()
    {
        GameObject go = Instantiate(bullet.gameObject, transform.position, Quaternion.identity);
        SpreadBullet goBullet = go.GetComponent<SpreadBullet>();

        // Set the bullet's velocity towards the player
        go.GetComponent<Rigidbody2D>().velocity = direction * fireForce;
        
    }
}
