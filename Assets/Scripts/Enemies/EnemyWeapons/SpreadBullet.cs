using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadBullet : MonoBehaviour
{
    public Vector2 direction;

    [SerializeField]
    private float fireForce = 10f;

    private Vector2 velocity;

    private void Start()
    {
        Destroy(gameObject, 4f);
    }

    private void Update()
    {
        velocity = direction * fireForce;
    }
}
