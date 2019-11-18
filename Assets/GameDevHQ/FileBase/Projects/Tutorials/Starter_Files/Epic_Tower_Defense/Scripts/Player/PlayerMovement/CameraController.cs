using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10f;
    private float _moveZ, _moveY;
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

        Vector3 activePos = transform.position;
        activePos.x = Mathf.Clamp(activePos.x, -29f, -1f);
        activePos.z = Mathf.Clamp(activePos.z, -7f, 10f);
        transform.position = activePos;
    }
}
