using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tower
{
    public int damage;
    public int cost;
    public TowerType tType;

    public enum TowerType
    {
        Gattling_Gun, // int 0
        Missile_Turret, // int 1
        Dual_Gattling_Gun, // int 2
        Dual_Missile_Turret // int 3
    }
}