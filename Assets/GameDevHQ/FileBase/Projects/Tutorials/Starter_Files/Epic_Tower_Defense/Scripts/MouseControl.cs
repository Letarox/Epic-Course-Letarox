using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
    [SerializeField]
    private float _speed = 2.5f, _maxSpeed = 5f;
    private float _width, _height, _validLeft, _maxLeft, _validRight, _maxRight, _validTop, _maxTop, _validBottom, _maxBottom;
    private float _limitX, _limitY;
    private Vector3 _mousePos, _cameraPos;
    private float _maxZoom = 15f, _minZoom = 30f;
    void Start()
    {
        _width = Screen.width;
        _height = Screen.height;
        _validLeft = _width * 0.1f; // values between 0 and 10% of the width
        _validRight = _width * 0.9f; // values between 90% to 100% of the width
        _validTop = _height * 0.9f; // values between 0 and 10% of the height
        _validBottom = _height * 0.1f; // values between 0 and 10% of the height

        _maxLeft = _width * 0.03f; // values between 0 and 3% of the width for faster movement at the edge of the screen
        _maxRight = _width * 0.97f; // values between 0 and 3% of the width for faster movement at the edge of the screen
        _maxTop = _height * 0.97f; // values between 0 and 3% of the height for faster movement at the edge of the screen
        _maxBottom = _height * 0.03f; // values between 0 and 3% of the height for faster movement at the edge of the screen
    }

    // Update is called once per frame
    void Update()
    {
        MouseMovement();
        MouseZoom();
    }

    private void MouseZoom()
    {
        _cameraPos = transform.position;
        _cameraPos.y -= Input.mouseScrollDelta.y;
        if (_cameraPos.y < _maxZoom)
            _cameraPos.y = _maxZoom;
        if (_cameraPos.y > _minZoom)
            _cameraPos.y = _minZoom;
        transform.position = _cameraPos;
    }

    private void MouseMovement()
    {
        _mousePos = Input.mousePosition;
        if(_mousePos.x < _validLeft)
        {
            if(_mousePos.x < _maxLeft)
            {
                transform.Translate(Vector3.left * _maxSpeed * Time.deltaTime);
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
                transform.Translate(Vector3.right * _maxSpeed * Time.deltaTime);
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
                transform.Translate(Vector3.up * _maxSpeed * Time.deltaTime);
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
                transform.Translate(Vector3.down * _maxSpeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.down * _speed * Time.deltaTime);
            }
        }
    }
}
