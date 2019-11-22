using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour, IDamageble
{
    [SerializeField]
    private GameObject _target;
    [SerializeField]
    private NavMeshAgent _agent;
    [SerializeField]
    private float _speed;
    private Animator _anim;

    [SerializeField]
    private EnemyType _enemyType;

    [SerializeField]
    private GameObject _explosionPrefab;

    public int Health { get; set; }
    public int Warfunds { get; set; }
    public int LivesCost { get; set; }
    public float Speed { get; set; }

    enum EnemyType
    {
        Tall_Mech,
        Big_Mech
    }

    void SetStats()
    {
        switch (_enemyType)
        {
            case EnemyType.Tall_Mech:
                Health = 100;
                Warfunds = Random.Range(30, 51);
                LivesCost = 1;
                Speed = 2.2f;
                break;
            case EnemyType.Big_Mech:
                Health = 150;
                Warfunds = Random.Range(45, 76);
                LivesCost = 2;
                Speed = 1.8f;
                break;
            default:
                break;
        }
    }

    void OnEnable()
    {
        SetStats();
        _agent = GetComponent<NavMeshAgent>();
        _target = GameObject.Find("Player_Base");
        _anim = GetComponent<Animator>();

        if (_agent == null)
            Debug.LogError("NavMesh Agent is NULL on " + transform.name);

        if (_target == null)
            Debug.LogError("Player base is NULL on enemy: " + transform.name);
        else
            _agent.SetDestination(_target.transform.position);

        if (_anim == null)
            Debug.LogError("Animator is NULL on " + transform.name);

        _agent.speed = Speed;
        _speed = _agent.speed;
    }

    void Update()
    {

    }

    void OnValidate()
    {
        _agent.speed = _speed;
    }

    void Explode()
    {
        GameObject gO = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gO, 5f);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public int ReturnEnemyType()
    {
        return (int)_enemyType;
    }

    public void Damage(GameObject source, int damageAmount)
    {
        Health -= damageAmount;
        if (Health <= 0)
        {
            ITower towerScript = source.GetComponent<ITower>();
            if(towerScript != null)
                towerScript.CleanTarget();
            GameManager.Instance.AddFunds(Warfunds);
            StartCoroutine(DeathRoutine());
        }
    }

    IEnumerator DeathRoutine()
    {
        _anim.SetTrigger("Dead");
        _agent.speed = 0f;
        yield return new WaitForSeconds(0.75f);
        Hide();
        Explode();
    }
}
