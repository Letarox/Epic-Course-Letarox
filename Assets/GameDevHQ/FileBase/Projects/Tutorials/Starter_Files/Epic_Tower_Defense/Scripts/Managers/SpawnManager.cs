using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoSingleton<SpawnManager>
{
    [SerializeField]
    private Transform _spawnLocation;
    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private int _enemiesInitialWave;
    //[SerializeField]
    //private int _towersInitially;

    [SerializeField]
    private List<GameObject> _enemiesPrefabs;
    [SerializeField]
    private List<GameObject> _enemiesPool;

    //[SerializeField]
    //private List<GameObject> _towersPrefabs;
    //[SerializeField]
    //private List<GameObject> _towersPool;
    //[SerializeField]
    //private GameObject _towerContainer;
    
    private int _waveNumber = 1;

    private void Start()
    {
        _enemiesPool = GenerateEnemies(_enemiesInitialWave);
        //_towersPool = GenerateTower(_towersInitially);        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(WaveSpawn());
        }
    }

    IEnumerator WaveSpawn()
    {
        for(int i = 0; i < _waveNumber * 10; i++)
        {
            int randomEnemy = Random.Range(0, 2);
            GameObject enemy = RequestEnemy(randomEnemy);
            yield return new WaitForSeconds(1.5f);
        }        
    }

    List<GameObject> GenerateEnemies(int amountOfEnemies)
    {
        for(int i = 0; i < amountOfEnemies; i++)
        {
            int randomEnemy = Random.Range(0, 2);
            GameObject enemy = Instantiate(_enemiesPrefabs[randomEnemy], _spawnLocation);
            enemy.transform.parent = _enemyContainer.transform;
            enemy.SetActive(false);
            _enemiesPool.Add(enemy);
        }        

        return _enemiesPool;
    }

    public GameObject RequestEnemy(int type)
    {
        //check if the type is the same that we requested and return it
        foreach(var enemy in _enemiesPool)
        {
            AI enemyAI = enemy.GetComponent<AI>();
            if (enemyAI == null)
                Debug.LogError("EnemyAI is NULL on the Spawn Manager");
            if(enemy.activeInHierarchy == false && enemyAI.GetEnemyType() == type)
            {
                enemy.SetActive(true);
                enemy.transform.position = _spawnLocation.position;
                return enemy;
            }
        }

        GameObject newEnemy = Instantiate(_enemiesPrefabs[type], _spawnLocation);
        newEnemy.transform.parent = _enemyContainer.transform;
        newEnemy.SetActive(true);
        _enemiesPool.Add(newEnemy);
        newEnemy.transform.position = _spawnLocation.position;

        return newEnemy;
    }

    /*List<GameObject> GenerateTower(int amountOfTowers)
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
    }*/
}
