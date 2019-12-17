using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerBase : MonoBehaviour
{
    public static Action OnTakingDamage;
    private void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            AI enemyAI = other.GetComponent<AI>();
            if(enemyAI != null)
            {
                GameManager.Instance.ChangeLives(enemyAI.LivesCost);
                if (OnTakingDamage != null)
                    OnTakingDamage();
                enemyAI.Hide();
            }            
        }
    }
}
