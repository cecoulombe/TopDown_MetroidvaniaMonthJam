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
        //go.GetComponent<Rigidbody2D>().velocity = direction * fireForce;
        go.GetComponent<Rigidbody2D>().AddForce(fireForce * direction, ForceMode2D.Impulse);
        //SpreadBullet goBullet = go.GetComponent<SpreadBullet>();

        // Set the bullet's velocity towards the player
        //go.GetComponent<Rigidbody2D>().velocity = direction * fireForce;

        Destroy(go, 4f);
        
    }
}
