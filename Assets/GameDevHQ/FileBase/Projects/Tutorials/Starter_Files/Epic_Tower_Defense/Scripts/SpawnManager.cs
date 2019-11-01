using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private static SpawnManager _instance;
    public static SpawnManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("Instance is NULL on the SpawnManager");

            return _instance;
        }
    }
    void Awake()
    {
        _instance = this;
    }

    [SerializeField]
    private Transform _spawnLocation, _playerBase;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private List<GameObject> _enemiesPrefabs;
    [SerializeField]
    private List<GameObject> _enemiesPool;

    private int waveNumber = 1;
    
    void Start()
    {
        _enemiesPool = GenerateEnemies(10);
        StartCoroutine(WaveSpawn());
    }

    void Update()
    {
        
    }

    IEnumerator WaveSpawn()
    {
        for(int i = 0; i < waveNumber * 10; i++)
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
            if(enemy.activeInHierarchy == false && enemyAI._enemy.type == type)
            {
                enemy.SetActive(true);
                return enemy;
            }
        }

        GameObject newEnemy = Instantiate(_enemiesPrefabs[type]);
        newEnemy.transform.parent = _enemyContainer.transform;
        newEnemy.SetActive(true);
        _enemiesPool.Add(newEnemy);

        return newEnemy;
    }
}
