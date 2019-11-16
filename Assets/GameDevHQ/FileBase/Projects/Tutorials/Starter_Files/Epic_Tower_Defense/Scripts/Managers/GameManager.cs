using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField]
    private int _warFunds;
    [SerializeField]
    private int _lives = 5;

    private bool _isGameOver = false;

    public void AddFunds(int fundsAmount)
    {
        _warFunds += fundsAmount;
    }

    public void RemoveFunds(int fundsAmount)
    {
        _warFunds -= fundsAmount;
    }

    public void ChangeLives(int livesAmouht)
    {
        _lives -= livesAmouht;
        if (_lives <= 0)
        {
            _isGameOver = true;
            Time.timeScale = 0;
        }
    }

    public int GetFunds()
    {
        return _warFunds;
    }
}
