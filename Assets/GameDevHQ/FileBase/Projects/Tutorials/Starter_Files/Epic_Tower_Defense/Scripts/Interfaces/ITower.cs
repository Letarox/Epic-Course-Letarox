using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITower
{
    int Damage { get; set; }
    int WarfundCost { get; set; }
    float FireRate { get; set; }
    int GetTowerType();
    void Hide();

    void Shoot(GameObject target);
}
