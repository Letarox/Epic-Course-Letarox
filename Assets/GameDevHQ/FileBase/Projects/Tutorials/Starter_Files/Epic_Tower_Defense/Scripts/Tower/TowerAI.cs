using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAI : MonoBehaviour
{
    [SerializeField]
    private Tower _tower;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public int GetTowerType()
    {
        return (int)_tower.tType;
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}