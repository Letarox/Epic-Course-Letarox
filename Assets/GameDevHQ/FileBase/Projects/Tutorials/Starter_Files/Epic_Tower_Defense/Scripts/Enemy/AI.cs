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

    [SerializeField]
    private EnemyType _enemyType;

    public int Health { get; set; }
    public int Warfunds { get; set; }

    enum EnemyType
    {
        Tall_Mech,
        Big_Mech
    }

    void Awake()
    {
        switch (_enemyType)
        {
            case EnemyType.Tall_Mech:
                Health = 100;
                Warfunds = Random.Range(30, 51);
                break;
            case EnemyType.Big_Mech:
                Health = 150;
                Warfunds = Random.Range(45, 76);
                break;
            default:
                break;
        }
    }

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _target = GameObject.Find("Player_Base");
        if (_target == null)
        {
            Debug.LogError("Player base is NULL on enemy: " + transform.name);
        }
        else
        {
            _agent.SetDestination(_target.transform.position);
        }

        _speed = _agent.speed;
    }

    void Update()
    {

    }

    void OnValidate()
    {
        _agent.speed = _speed;
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public int ReturnEnemyType()
    {
        return (int)_enemyType;
    }

    public void Damage(int damageAmount)
    {
        Health -= damageAmount;
        if (Health <= 0)
        {
            GameManager.Instance.AddFunds(Warfunds);
            Hide();
        }
    }
}
