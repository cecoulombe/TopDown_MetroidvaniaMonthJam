using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadBullet : MonoBehaviour
{
    public Vector2 direction;

    [SerializeField]
    private float fireForce = 10f;

    private Vector2 velocity;

    private void Update()
    {
        velocity = direction * fireForce;
    }

    private void FixedUpdate()
    {
        // i don't like this, I don't think it will work for what I am doing
        Vector2 pos = transform.position;

        pos += velocity * Time.deltaTime;

        transform.position = pos;
    }
}
