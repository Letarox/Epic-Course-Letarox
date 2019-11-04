using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Enemy
{
    public int health;
    public int warfunds;
    public EnemyType eType;
    
    public enum EnemyType
    {
        TallMech, // int 0
        BigMech // int 1
    }

    public EnemyType ReturnEnemyType()
    {
        return eType;
    }
}
