using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    [SerializeField]
    private GameObject _target;
    private NavMeshAgent _agent;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private Enemy _enemy;

    private Player _player;    

    private void OnEnable()
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

        _player = GameObject.Find("Main Camera").GetComponent<Player>();

        if (_player == null)
        {
            Debug.LogError("Player is null on enemy: " + transform.name);
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
            _player.AddFunds(_enemy.warfunds);
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
