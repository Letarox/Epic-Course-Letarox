﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageble
{
    int Health { get; set; }
    int Warfunds { get; set; }
    float Speed { get; set; }
    void Damage(int damageAmount);
}
