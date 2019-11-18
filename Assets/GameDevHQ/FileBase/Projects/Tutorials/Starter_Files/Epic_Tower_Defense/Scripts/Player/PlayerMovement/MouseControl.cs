using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    private float _width, _height, _validLeft, _maxLeft, _validRight, _maxRight, _validTop, _maxTop, _validBottom, _maxBottom;
    private Vector3 _mousePos, _cameraPos, direction = new Vector3(0,0,0);
    private float _maxZoom = 18f, _minZoom = 30f;
    void Start()
    {
        _width = Screen.width;
        _height = Screen.height;
        _validLeft = _width * 0.1f; // values between 0 and 10% of the width
        _validRight = _width * 0.9f; // values between 90% to 100% of the width
        _validTop = _height * 0.9f; // values between 90 and 100% of the height
        _validBottom = _height * 0.1f; // values between 0 and 10% of the height

        _maxLeft = _width * 0.03f; // values between 0 and 3% of the width for faster movement at the edge of the screen
        _maxRight = _width * 0.97f; // values between 97 and 100% of the width for faster movement at the edge of the screen
        _maxTop = _height * 0.97f; // values between 97 and 100% of the height for faster movement at the edge of the screen
        _maxBottom = _height * 0.03f; // values between 0 and 3% of the height for faster movement at the edge of the screen
    }

    void Update()
    {
        MouseMovement();
        MouseZoom();
    }

    private void MouseZoom()
    {
        _cameraPos = transform.position;
        _cameraPos.y -= Input.mouseScrollDelta.y;
        _cameraPos.y = Mathf.Clamp(_cameraPos.y, _maxZoom, _minZoom);
        transform.position = _cameraPos;
    }

    private void MouseMovement()
    {
        _mousePos = Input.mousePosition;
        direction = Vector3.zero;
        if(_mousePos.x < _validLeft)
        {
            direction.z = -1f;
            if (_mousePos.x < _maxLeft)
            {
                _speed = 5f;
            }
            else
            {
                _speed = 2.5f;
            }            
        }
        else if(_mousePos.x > _validRight)
        {
            direction.z = 1f;
            if (_mousePos.x > _maxRight)
            {
                _speed = 5f;
            }
            else
            {
                _speed = 2.5f;
            }
        }

        if(_mousePos.y > _validTop)
        {
            direction.x = -1f;
            if (_mousePos.y > _maxTop)
            {
                _speed = 5f;
            }
            else
            {
                _speed = 2.5f;
            }
        }
        else if(_mousePos.y < _validBottom)
        {
            direction.x = 1f;
            if (_mousePos.y < _maxBottom)
            {
                _speed = 5f;
            }
            else
            {
                _speed = 2.5f;
            }
        }

        transform.Translate(direction * _speed * Time.deltaTime);
    }
}
