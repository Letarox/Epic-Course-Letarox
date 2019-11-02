using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    [SerializeField]
    private GameObject _target;
    private NavMeshAgent _agent;
    public Enemy _enemy;
    void Start()
    {
        
    }

    void OnEnable()
    {
        _agent = GetComponent<NavMeshAgent>();
        _target = GameObject.Find("Player_Base");
        _agent.SetDestination(_target.transform.position);
        Debug.Log("Working... " + transform.name);
    }

    void Update()
    {
        if(_enemy.health <= 0)
        {
            Hide();
        }
    }

    void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
