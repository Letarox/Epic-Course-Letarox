using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int _warFunds;
    [SerializeField]
    private int _lives = 5;

    private bool _isGameOver = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddFunds(int fundsAmount)
    {
        _warFunds += fundsAmount;
    }

    public void ChangeLives(int livesAmouht)
    {
        _lives -= livesAmouht;
        if(_lives <= 0)
        {
            _isGameOver = true;
            Time.timeScale = 0;
        }
    }
}
