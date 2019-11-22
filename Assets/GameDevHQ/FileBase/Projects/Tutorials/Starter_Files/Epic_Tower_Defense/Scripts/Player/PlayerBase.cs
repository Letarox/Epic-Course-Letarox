using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
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
                enemyAI.Hide();
            }            
        }
    }
}
