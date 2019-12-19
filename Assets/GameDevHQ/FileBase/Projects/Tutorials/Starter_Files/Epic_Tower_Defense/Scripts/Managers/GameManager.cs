using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField]
    private int _warFunds;
    [SerializeField]
    private int _lives = 20;

    private bool _isPaused = false;
    private bool _gameStarted = false;
    private int _fastForwardSpeed = 1;

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
        {
            Time.timeScale = 0;
            UIMananger.Instance.GameOver();
        }            
    }

    public void StartGame()
    {
        _gameStarted = true;
    }

    public void ControlTime(int utility)
    {
        switch(utility)
        {
            case 0:
                if(_gameStarted == true)
                {
                    _isPaused = true;
                    Time.timeScale = 0;
                }                
                break;
            case 1:
                if (_gameStarted == false)
                {
                    UIMananger.Instance.GameStart();
                }
                else
                {
                    _isPaused = false;
                    _fastForwardSpeed = 1;
                    Time.timeScale = _fastForwardSpeed;
                }
                break;
            case 2:
                if (_isPaused == false && _gameStarted == true)
                {
                    _fastForwardSpeed += 2;
                    if (_fastForwardSpeed >= 8)
                        _fastForwardSpeed = 8;
                    Time.timeScale = _fastForwardSpeed;
                }
                break;
            default:
                break;
        }
    }

    public int GetFunds()
    {
        return _warFunds;
    }
    public int GetLives()
    {
        return _lives;
    }
}
