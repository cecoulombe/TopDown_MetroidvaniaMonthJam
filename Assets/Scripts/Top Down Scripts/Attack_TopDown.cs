using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_TopDown : MonoBehaviour
{
    #region Variables
    public GameObject Melee;

    [SerializeField]
    private bool isAttacking;

    [SerializeField]
    private float timeToAttack = 0.3f;

    [SerializeField]
    private float attackDuration = 0.3f;

    [SerializeField]
    private float attackTimer = 0f;


    #endregion


    // Update is called once per frame
    void Update()
    {
        CheckMeleeTimer();

        if (Input.GetKeyDown(KeyCode.K))
        {
            OnAttack();
        }
    }

    private void OnAttack()
    {
        if(!isAttacking)
        {
            Melee.SetActive(true);
            isAttacking = true;
            // call your animator to play your melee attack
        }
    }

    private void CheckMeleeTimer()
    {
        if(isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if(attackTimer >= attackDuration)
            {
                attackTimer = 0;
                isAttacking = false;
                Melee.SetActive(false);
            }
        }
    }
}
