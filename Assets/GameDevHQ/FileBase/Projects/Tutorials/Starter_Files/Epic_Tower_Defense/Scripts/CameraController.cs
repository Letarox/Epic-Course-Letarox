using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    private float _limitLeft = -38f, _limitRight = -34f, _limitUp = 27f, _limitDown = 12f;
    private float _moveX, _moveY;
    private Vector3 _cameraPos;
    void Start()
    {
        
    }

    void Update()
    {
        //get the input from WASD keys
        _moveX = Input.GetAxis("Horizontal");
        _moveY = Input.GetAxis("Vertical");

        //move the camera based on the keys
        transform.Translate(new Vector3(_moveX, _moveY, 0));
    }
}
