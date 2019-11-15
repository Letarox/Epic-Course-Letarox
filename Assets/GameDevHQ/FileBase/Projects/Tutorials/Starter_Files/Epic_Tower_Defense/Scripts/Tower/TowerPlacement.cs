using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TowerPlacement : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _decoyTowers = new GameObject[2];
    [SerializeField]
    private GameObject[] _towers = new GameObject[2];
    private GameObject _activeTowerMouseDrag, _currentPlacingTower;
    private bool _isSummoning = false;
    private bool _isRemoving = false;
    private int _towerType;

    public static event Action onAvailableOn;
    public static event Action onAvailableOff;
    public static event Action onSaleOn;
    public static event Action onSaleOff;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1) && _isSummoning == false && _isRemoving == false)
        {
            _isSummoning = true;
            _activeTowerMouseDrag = _decoyTowers[0];
            _activeTowerMouseDrag.SetActive(true);
            _currentPlacingTower = _towers[0];
            _towerType = 0;
            if (onAvailableOn != null)
                onAvailableOn();
        }

        if(Input.GetKeyDown(KeyCode.Alpha2) && _isSummoning == false && _isRemoving == false)
        {
            _isSummoning = true;
            _activeTowerMouseDrag = _decoyTowers[1];
            _activeTowerMouseDrag.SetActive(true);
            _currentPlacingTower = _towers[1];
            _towerType = 1;
            if (onAvailableOn != null)
                onAvailableOn();
        }        

        if(Input.GetKeyDown(KeyCode.Alpha3) && _isSummoning == false && _isRemoving == false)
        {
            _isRemoving = true;
            if (onSaleOn != null)
                onSaleOn();
        }

        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButton(1))
        {
            if(_isSummoning == true)
            {
                _isSummoning = false;
                if (onAvailableOff != null)
                    onAvailableOff();
            }
            if(_isRemoving == true)
            {
                _isRemoving = false;
                if (onSaleOff != null)
                    onSaleOff();
            }
            _activeTowerMouseDrag.SetActive(false);
        }

        TowerSummon();
        TowerRemove();        
    }

    void TowerSummon()
    {
        if(_isSummoning == true)
        {
            Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(rayOrigin, out hitInfo))
            {
                Vector3 target = hitInfo.point;
                target.y = 0f;
                _activeTowerMouseDrag.transform.position = target;

                if(Input.GetMouseButton(0))
                {
                    if(hitInfo.transform.tag == "TowerSpot")
                    {
                        TowerSpot towerSpot = hitInfo.transform.GetComponent<TowerSpot>();
                        if(hitInfo.transform.GetComponent<TowerSpot>() != null)
                        {
                            if (towerSpot.GetSpotAvailability() == false)
                            {
                                GameObject newTower = SpawnManager.Instance.RequestTower(_towerType, hitInfo.point);
                                towerSpot.SetTower(newTower);
                            }
                        }                        
                    }
                }
            }
        }
    }

    void TowerRemove()
    {
        if (_isRemoving == true)
        {
            Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(rayOrigin, out hitInfo))
            {
                if (Input.GetMouseButton(0))
                {
                    if (hitInfo.transform.tag == "TowerSpot")
                    {
                        if (hitInfo.transform.GetComponent<TowerSpot>() != null)
                            hitInfo.transform.GetComponent<TowerSpot>().SellTower();
                    }
                }
            }
        }
    }
}