using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_TopDown : MonoBehaviour
{
    #region Varaibles
    [SerializeField]
    private float moveSpeed = 2f;

    private Rigidbody2D rb;
    private Transform target;
    private Vector2 moveDirection;

    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        target = GameObject.Find("Player").transform;
    }

    void Update()
    {
        if(target)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            moveDirection = direction;
        }
    }

    private void FixedUpdate()
    {
        if(target)
        {
            rb.velocity = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed;
        }
    }
}
