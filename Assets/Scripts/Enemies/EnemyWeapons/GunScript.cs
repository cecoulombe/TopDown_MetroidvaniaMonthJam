using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{

    public SpreadBullet bullet;

    Vector2 direction;

    private void Update()
    {
        direction = (transform.localRotation * Vector3.forward).normalized;
    }

    public void Shoot()
    {
        GameObject go = Instantiate(bullet.gameObject, transform.position, Quaternion.identity);
        SpreadBullet goBullet = go.GetComponent<SpreadBullet>();

        goBullet.direction = direction;
    }
}
