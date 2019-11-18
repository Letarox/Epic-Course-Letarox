using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TowerPlacement : MonoSingleton<TowerPlacement>
{
    [SerializeField]
    private GameObject[] _decoyTowers = new GameObject[2];
    private GameObject _activeTowerMouseDrag;
    private bool _isSummoning = false;
    private bool _isRemoving = false;
    [SerializeField]
    private bool _onSpot = false;
    private int _towerType;

    public static event Action<bool> OnAvailable;
    public static event Action<bool> OnSale;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1) && _isSummoning == false && _isRemoving == false)
        {
            _isSummoning = true;
            _activeTowerMouseDrag = _decoyTowers[0];
            _activeTowerMouseDrag.SetActive(true);
            _towerType = 0;
            if (OnAvailable != null)
                OnAvailable(true);
        }

        if(Input.GetKeyDown(KeyCode.Alpha2) && _isSummoning == false && _isRemoving == false)
        {
            _isSummoning = true;
            _activeTowerMouseDrag = _decoyTowers[1];
            _activeTowerMouseDrag.SetActive(true);
            _towerType = 1;
            if (OnAvailable != null)
                OnAvailable(true);
        }        

        if(Input.GetKeyDown(KeyCode.Alpha3) && _isSummoning == false && _isRemoving == false)
        {
            _isRemoving = true;
            if (OnSale != null)
                OnSale(true);
        }

        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButton(1))
        {
            if(_isSummoning == true)
            {
                _isSummoning = false;
                if (OnAvailable != null)
                    OnAvailable(false);
            }
            if(_isRemoving == true)
            {
                _isRemoving = false;
                if (OnSale != null)
                    OnSale(false);
            }
            if(_activeTowerMouseDrag != null)
                _activeTowerMouseDrag.SetActive(false);
        }

        TowerSummon();
        TowerRemove();        
    }

    void TowerSummon()
    {
        if(_isSummoning == true && _onSpot == false)
        {
            Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(rayOrigin, out hitInfo))
            {
                Vector3 target = hitInfo.point;
                target.y = 0f;
                _activeTowerMouseDrag.transform.position = target;
            }
        }
    }

    void TowerRemove()
    {
        if (_isRemoving == true && _onSpot == false)
        {
            Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
        }
    }

    public void StayAtSpotPosition(Vector3 position)
    {
        _activeTowerMouseDrag.transform.position = position;
    }

    public void SpotCheck(bool active)
    {
        _onSpot = active;
    }

    public bool GetSumonning()
    {
        return _isSummoning;
    }

    public bool GetRemoving()
    {
        return _isRemoving;
    }

    public bool GetSpotValue()
    {
        return _onSpot;
    }

    public int GetTowerType()
    {
        return _towerType;
    }
}