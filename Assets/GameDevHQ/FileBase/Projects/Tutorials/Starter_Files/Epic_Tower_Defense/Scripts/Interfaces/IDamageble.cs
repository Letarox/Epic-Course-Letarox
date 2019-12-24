using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageble
{
    int Health { get; set; }
    int Warfunds { get; set; }
    int LivesCost { get; set; }
    float Speed { get; set; }
    void Damage(ITower source, int damageAmount);
}
