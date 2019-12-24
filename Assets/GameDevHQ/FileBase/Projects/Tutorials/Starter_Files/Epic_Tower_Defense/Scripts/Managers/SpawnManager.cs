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
    [SerializeField]
    private int _missilesInitially;
    //[SerializeField]
    //private int _towersInitially;

    [SerializeField]
    private List<GameObject> _enemiesPrefabs;
    [SerializeField]
    private List<GameObject> _enemiesPool;

    [SerializeField]
    private List<GameObject> _explosionsPrefabs;
    [SerializeField]
    private List<GameObject> _explosionsPool;
    [SerializeField]
    private GameObject _explosionContainer;

    [SerializeField]
    private GameObject _missilePrefab;
    [SerializeField]
    private List<GameObject> _missilePool;
    [SerializeField]
    private GameObject _missileContainer;


    //[SerializeField]
    //private List<GameObject> _towersPrefabs;
    //[SerializeField]
    //private List<GameObject> _towersPool;
    //[SerializeField]
    //private GameObject _towerContainer;
    
    private int _waveNumber = 1;
    private WaitForSeconds _waveSpawnDelay = new WaitForSeconds(1.5f);

    private void Start()
    {
        _enemiesPool = GenerateEnemies(_enemiesInitialWave);
        //_towersPool = GenerateTower(_towersInitially);        
        _explosionsPool = GenerateExplosions();
        _missilePool = GenerateMissiles(_missilesInitially);
    }

    public void StartGame()
    {
        StartCoroutine(WaveSpawn());
    }

    IEnumerator WaveSpawn()
    {
        for(int i = 0; i < _waveNumber * 10; i++)
        {
            int randomEnemy = Random.Range(0, 2);
            GameObject enemy = RequestEnemy(randomEnemy);
            yield return _waveSpawnDelay;
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

    public Vector3 GetSpawnLocation()
    {
        return _spawnLocation.position;
    }

    List<GameObject> GenerateExplosions()
    {
        for (int i = 0; i < _explosionsPrefabs.Count; i++)
        {
            GameObject explosion = Instantiate(_explosionsPrefabs[i]);
            explosion.transform.parent = _explosionContainer.transform;
            explosion.SetActive(false);
            _explosionsPool.Add(explosion);
        }

        return _explosionsPool;
    }

    public GameObject RequestExplosion(int type, GameObject requester)
    {
        //pass myself as a parameter to request for the explosion type
        foreach (var explosion in _explosionsPool)
        {
            Explosion explosionScript = explosion.GetComponent<Explosion>();
            if (explosionScript == null)
                Debug.LogError("Explosion Script is NULL on " + transform.name);
            if (explosion.activeInHierarchy == false && explosionScript.GetExplosionType() == type)
            {
                explosion.SetActive(true);
                explosion.transform.position = requester.transform.position;
                return explosion;
            }
        }

        GameObject newExplosion = Instantiate(_explosionsPrefabs[type], requester.transform);
        newExplosion.transform.parent = _explosionContainer.transform;
        newExplosion.SetActive(true);
        _explosionsPool.Add(newExplosion);
        newExplosion.transform.position = requester.transform.position;

        return newExplosion;
    }

    List<GameObject> GenerateMissiles(int amountOfMissiles)
    {
        for (int i = 0; i < amountOfMissiles; i++)
        {
            GameObject missile = Instantiate(_missilePrefab, _spawnLocation);
            missile.transform.parent = _missileContainer.transform;
            missile.SetActive(false);
            _missilePool.Add(missile);
        }

        return _missilePool;
    }

    public GameObject RequestMissile(GameObject parentPosition)
    {
        //check if the type is the same that we requested and return it
        foreach (var missile in _missilePool)
        {
            if (missile.activeInHierarchy == false)
            {
                missile.SetActive(true);
                missile.transform.position = parentPosition.transform.position;
                return missile;
            }
        }

        GameObject newMissile = Instantiate(_missilePrefab, parentPosition.transform);
        newMissile.transform.parent = _missileContainer.transform;
        newMissile.SetActive(true);
        _missilePool.Add(newMissile);
        newMissile.transform.position = parentPosition.transform.position;

        return newMissile;
    }

    public void ReAssignParent(GameObject missile)
    {
        missile.transform.parent = _missileContainer.transform;
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
