using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField]
    private int _warFunds;
    [SerializeField]
    private int _lives = 5;

    //private bool _isGameOver = false;

    public void SetEnemyStats(GameObject enemy)
    {
        AI enemyAI = enemy.GetComponent<AI>();
        if(enemyAI != null)
        {
            int type = enemyAI.GetEnemyType();
            switch (type)
            {
                case 0: //Tall Mech
                    enemyAI.Health = 100;
                    enemyAI.Warfunds = Random.Range(30, 51);
                    enemyAI.LivesCost = 1;
                    enemyAI.Speed = 2.2f;
                    break;
                case 1: //Big Mech
                    enemyAI.Health = 180;
                    enemyAI.Warfunds = Random.Range(45, 76);
                    enemyAI.LivesCost = 2;
                    enemyAI.Speed = 1.8f;
                    break;
                default:
                    break;
            }
        }
        
    }

    public void AddFunds(int fundsAmount)
    {
        _warFunds += fundsAmount;
    }

    public void RemoveFunds(int fundsAmount)
    {
        _warFunds -= fundsAmount;
    }

    public void ChangeLives(int livesAmount)
    {
        _lives -= livesAmount;

        if (_lives <= 0)
            Time.timeScale = 0;
    }

    public int GetFunds()
    {
        return _warFunds;
    }
}
