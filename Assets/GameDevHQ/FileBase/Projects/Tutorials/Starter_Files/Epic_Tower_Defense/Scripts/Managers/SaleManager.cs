using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaleManager : MonoSingleton<SaleManager>
{
    [SerializeField]
    private int _towersInitially;

    [SerializeField]
    private List<GameObject> _towersPrefabs;
    [SerializeField]
    private List<GameObject> _towersPool;
    [SerializeField]
    private GameObject _towerContainer;

    void Start()
    {
        _towersPool = GenerateTower(_towersInitially);
    }

    private int[] _towerTypeCost = new int[2] { 100, 150 };
    public int GetTowerCost(int type)
    {
        return _towerTypeCost[type];
    }

    List<GameObject> GenerateTower(int amountOfTowers)
    {
        for (int i = 0; i < amountOfTowers; i++)
        {
            int random = Random.Range(0, 2);
            GameObject tower = Instantiate(_towersPrefabs[random]);
            tower.transform.parent = _towerContainer.transform;
            tower.SetActive(false);
            _towersPool.Add(tower);
        }

        return _towersPool;
    }

    public GameObject RequestTower(int type, Vector3 target)
    {
        //check if the type is the same that we requested and return it
        foreach (var tower in _towersPool)
        {
            TowerAI towerAI = tower.GetComponent<TowerAI>();
            if (towerAI == null)
                Debug.LogError("TowerAI is NULL on the Spawn Manager");
            if (tower.activeInHierarchy == false && towerAI.GetTowerType() == type)
            {
                tower.SetActive(true);
                tower.transform.position = target;
                return tower;
            }
        }

        GameObject newTower = Instantiate(_towersPrefabs[type], target, Quaternion.identity);
        newTower.transform.parent = _towerContainer.transform;
        newTower.SetActive(true);
        _towersPool.Add(newTower);
        newTower.transform.position = target;

        return newTower;
    }
}
