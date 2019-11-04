using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    private Player _player;
    void Start()
    {
        _player = GameObject.Find("Main Camera").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is NULL on the Player Base");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            _player.ChangeLives(1);
            other.GetComponent<AI>().Hide();
        }
    }
}
