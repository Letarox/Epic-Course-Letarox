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
    public void SetTowerStats(GameObject tower)
    {
        ITower towerAI = tower.GetComponent<ITower>();
        if(towerAI != null)
        {
            int type = towerAI.GetTowerType();
            switch (type)
            {
                case 0: // Gattling Gun
                    towerAI.Damage = 10;
                    towerAI.WarfundCost = 100;
                    towerAI.SellAmount = 80;
                    towerAI.FireRate = 0.25f;
                    break;
                case 1: //Missile Turret
                    towerAI.Damage = 60;
                    towerAI.WarfundCost = 150;
                    towerAI.SellAmount = 120;
                    towerAI.FireRate = 2.5f;
                    break;
                case 2: //Dual Gatting Gun
                    towerAI.Damage = 20;
                    towerAI.WarfundCost = 400;
                    towerAI.SellAmount = 320;
                    towerAI.FireRate = 0.25f;
                    break;
                case 3: //Dual Missile Turret
                    towerAI.Damage = 60;
                    towerAI.WarfundCost = 500;
                    towerAI.SellAmount = 400;
                    towerAI.FireRate = 2.25f;
                    break;
                default:
                    break;
            }
        }        
    }
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
                    enemyAI.Warfunds = Random.Range(10, 26);
                    enemyAI.LivesCost = 1;
                    enemyAI.Speed = 2.2f;
                    break;
                case 1: //Big Mech
                    enemyAI.Health = 180;
                    enemyAI.Warfunds = Random.Range(20, 51);
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
