using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    [SerializeField]
    private GameObject _target;
    [SerializeField]
    private NavMeshAgent _agent;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private Enemy _enemy;

    private void Start()
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _enemy.health -= 10;
        }

        if (_enemy.health <= 0)
        {
            GameManager.Instance.AddFunds(_enemy.warfunds);
            Hide();
        }
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public int ReturnEnemyType()
    {
        return (int)_enemy.eType;
    }

    private void OnValidate()
    {
        _agent.speed = _speed;
    }

}
