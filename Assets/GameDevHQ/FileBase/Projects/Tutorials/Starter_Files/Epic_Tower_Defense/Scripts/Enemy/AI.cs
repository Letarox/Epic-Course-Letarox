using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GameDevHQ.FileBase.Missile_Launcher.Missile;

public class AI : MonoBehaviour, IDamageble
{
    [SerializeField]
    private GameObject _target;
    [SerializeField]
    private NavMeshAgent _agent;
    [SerializeField]
    private float _speed;
    private Animator _anim;
    private Collider _collider;

    [SerializeField]
    private EnemyType _enemyType;

    private GameObject _fireTarget;

    [SerializeField]
    private GameObject _body;
    Vector3 _defaultRot;

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

    [System.Obsolete]
    void OnEnable()
    {
        GameManager.Instance.SetEnemyStats(this.gameObject);
        _agent = GetComponent<NavMeshAgent>();
        _target = GameObject.Find("Player_Base");
        _anim = GetComponent<Animator>();
        _collider = GetComponent<Collider>();

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

        _defaultRot = _body.transform.localEulerAngles;

        transform.position = SpawnManager.Instance.GetSpawnLocation();

        _collider.enabled = true;
        _agent.speed = Speed;
        _speed = _agent.speed;
        Missile.OnMissileExplode += OnMissileDamageReceived;
    }

    void OnDisable()
    {
        Missile.OnMissileExplode -= OnMissileDamageReceived;
    }

    void OnValidate()
    {
        _agent.speed = _speed;
    }

    void Explode()
    {
        GameObject explosion = SpawnManager.Instance.RequestExplosion((1+(int)_enemyType), this.gameObject); //Base is 1 + type since the small explosion is being used by the missile
    }

    void OnMissileDamageReceived(Collider[] enemiesHit, ITower source)
    {
        foreach(var enemy in enemiesHit)
        {
            if (enemy == this._collider)
            {
                Damage(source.Damage, source);
            }
        }
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public int GetEnemyType()
    {
        return (int)_enemyType;
    }

    public void Damage(int explosionRadiusDamage, ITower source)
    {
        Health -= explosionRadiusDamage;
        if (Health <= 0)
        {
            if (source != null)
            {
                source.CleanTarget();
                GameManager.Instance.AddFunds(Warfunds);
                UIMananger.Instance.UpdateWarfunds(GameManager.Instance.GetFunds());
                if (this.gameObject.activeInHierarchy == true)
                    StartCoroutine(DeathRoutine());
            }
        }
    }

    public void Damage(ITower source, int damageAmount)
    {
        Health -= damageAmount;
        if (Health <= 0)
        {
            if(source != null)
            {
                source.CleanTarget();
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

    void OnTriggerEnter(Collider other)
    {
        if (_fireTarget == null)
        {
            if (other.tag == "Player")
            {
                _target = other.gameObject;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(_fireTarget == null)
        {
            _fireTarget = other.gameObject;
        }
        else
        {
            Vector3 direction = _fireTarget.transform.position - transform.position;
            if (_enemyType == EnemyType.Tall_Mech)
                _body.transform.LookAt(direction);
            else if(_enemyType == EnemyType.Big_Mech)
                _body.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            Vector3 rot = _body.transform.localEulerAngles;
            rot.x = 0f;
            if (_enemyType == EnemyType.Tall_Mech)
                rot.y = 0f;
            _body.transform.localEulerAngles = rot;
            _anim.SetTrigger("Shoot");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.Equals(_fireTarget))
        {
            _fireTarget = null;
            _anim.ResetTrigger("Shoot");
            _body.transform.localEulerAngles = _defaultRot;
        }
    }
}
