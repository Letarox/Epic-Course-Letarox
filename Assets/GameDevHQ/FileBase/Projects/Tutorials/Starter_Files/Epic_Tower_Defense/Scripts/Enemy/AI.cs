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
    private BoxCollider _collider;

    [SerializeField]
    private EnemyType _enemyType;

    [SerializeField]
    private GameObject[] _explosionPrefab;

    public int Health { get; set; }
    public int Warfunds { get; set; }
    public int LivesCost { get; set; }
    public float Speed { get; set; }

    enum EnemyType
    {
        Tall_Mech,
        Big_Mech
    }

    void OnEnable()
    {
        GameManager.Instance.SetEnemyStats(this.gameObject);
        _agent = GetComponent<NavMeshAgent>();
        _target = GameObject.Find("Player_Base");
        _anim = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider>();

        if (_agent == null)
            Debug.LogError("NavMesh Agent is NULL on " + transform.name);

        if (_target == null)
            Debug.LogError("Player base is NULL on enemy: " + transform.name);
        else
            _agent.SetDestination(_target.transform.position);

        if (_anim == null)
            Debug.LogError("Animator is NULL on " + transform.name);

        if (_collider == null)
            Debug.LogError("Box Collider is NULL on " + transform.name);

        transform.position = SpawnManager.Instance.GetSpawnLocation();

        _collider.enabled = true;
        _agent.speed = Speed;
        _speed = _agent.speed;
    }

    void OnValidate()
    {
        _agent.speed = _speed;
    }

    void Explode()
    {
        GameObject explosion = SpawnManager.Instance.RequestExplosion((1+(int)_enemyType), this.gameObject); //Base is 1 + type since the small explosion is being used by the missile
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public int GetEnemyType()
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
            {
                towerScript.CleanTarget();
                GameManager.Instance.AddFunds(Warfunds);
                UIMananger.Instance.UpdateWarfunds(GameManager.Instance.GetFunds());
                if(this.gameObject.activeInHierarchy == true)
                    StartCoroutine(DeathRoutine());
            }
        }
    }

    IEnumerator DeathRoutine()
    {
        _anim.SetTrigger("Dead");
        _collider.enabled = false;
        _agent.isStopped = true;
        float delay = _anim.GetCurrentAnimatorClipInfo(0).Length;
        yield return new WaitForSeconds(delay);
        Explode();
        Hide();
    }
}
