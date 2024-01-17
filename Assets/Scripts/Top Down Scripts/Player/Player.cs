using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    protected Rigidbody2D rb;

    private Vector2 moveInput;

    [Header("Movement Variables")]
    private float activeMoveSpeed;

    // how fast thel player should move
    [SerializeField] protected float walkSpeed;

    private void Start()
    {
        rb.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        moveInput = UserInput.instance.moveInput;

        rb.velocity = new Vector2(moveInput.x * activeMoveSpeed, moveInput.y * activeMoveSpeed);
    }
}


