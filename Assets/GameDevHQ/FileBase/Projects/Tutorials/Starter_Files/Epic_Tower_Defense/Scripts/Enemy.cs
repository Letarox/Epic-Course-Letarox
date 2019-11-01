using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Enemy
{
    [SerializeField]
    private float _speed = 1.5f;
    public int type;
    public int health;
    public int warfunds;

}
