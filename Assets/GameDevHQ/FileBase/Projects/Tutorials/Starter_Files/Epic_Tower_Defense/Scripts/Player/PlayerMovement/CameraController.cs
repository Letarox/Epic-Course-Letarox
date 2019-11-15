﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10f;
    private float _moveZ, _moveY;
    private float _limitLeft = -7f, _limitRight = 12f, _limitTop = -30f, _limitBottom = 1f;
    void Start()
    {

    }

    void Update()
    {
        //get the input from WASD keys
        _moveZ = Input.GetAxis("Horizontal");
        _moveY = Input.GetAxis("Vertical");

        //move the camera based on the keys
        Vector3 pos = new Vector3(-_moveY, 0, _moveZ);
        transform.Translate(pos * _speed * Time.deltaTime);
    }
}