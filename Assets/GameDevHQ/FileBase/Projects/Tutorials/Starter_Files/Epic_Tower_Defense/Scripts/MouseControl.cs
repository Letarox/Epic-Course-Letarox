﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
    [SerializeField]
    private float _speed = 2.5f;
    private float _width, _height, _validLeft, _maxLeft, _validRight, _maxRight, _validTop, _maxTop, _validBottom, _maxBottom;
    private Vector3 _mousePos;
    void Start()
    {
        _width = Screen.width;
        _height = Screen.height;
        _validLeft = _width * 0.1f; // values between 0 and 10% of the width
        _validRight = _width * 0.9f; // values between 90% to 100% of the width
        _validTop = _height * 0.9f; // values between 0 and 10% of the height
        _validBottom = _height * 0.1f; // values between 0 and 10% of the height

        _maxLeft = _width * 0.03f; // values to move faster than the start movement
        _maxRight = _width * 0.03f; // values to move faster than the start movement
        _maxTop = _height * 0.97f; // values to move faster than the start movement
        _maxBottom = _height * 0.03f; // values to move faster than the start movement
    }

    // Update is called once per frame
    void Update()
    {
        MouseMovement();
    }

    private void MouseMovement()
    {

        _mousePos = Input.mousePosition;
        if(_mousePos.x < _validLeft)
        {
            if(_mousePos.x < _maxLeft)
            {
                transform.Translate(Vector3.left * (2 * _speed) * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.left * _speed * Time.deltaTime);
            }            
        }
        else if(_mousePos.x > _validRight)
        {
            if (_mousePos.x > _maxRight)
            {
                transform.Translate(Vector3.right * (2 * _speed) * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.right * _speed * Time.deltaTime);
            }
        }

        if(_mousePos.y > _validTop)
        {
            if (_mousePos.y > _maxTop)
            {
                transform.Translate(Vector3.up * (2 * _speed) * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.up * _speed * Time.deltaTime);
            }
        }
        else if(_mousePos.y < _validBottom)
        {
            if (_mousePos.y < _maxBottom)
            {
                transform.Translate(Vector3.down * (2 * _speed) * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.down * _speed * Time.deltaTime);
            }
        }
    }
}
