using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpot : MonoBehaviour
{
    private ParticleSystem _greenParticle;
    private bool _isUsed = false;
    private GameObject _spotTower = null;
    private TowerAI _towerAI;

    [System.Obsolete]
    void Start()
    {
        _greenParticle = GetComponentInChildren<ParticleSystem>();
        if (_greenParticle == null)
            Debug.LogError("Green particle is NULL on " + transform.name);
    }

    void OnEnable()
    {
        TowerPlacement.onAvailableOn += TurnOnAvailable;
        TowerPlacement.onAvailableOff += TurnOffAvailable;
        TowerPlacement.onSaleOn += TurOnForSale;
        TowerPlacement.onSaleOff += TurOffForSale;
    }

    void OnDisable()
    {
        TowerPlacement.onAvailableOn -= TurnOnAvailable;
        TowerPlacement.onAvailableOff -= TurnOffAvailable;
        TowerPlacement.onSaleOn -= TurOnForSale;
        TowerPlacement.onSaleOff -= TurOffForSale;
    }

    void TurnOnAvailable()
    {
        if(_isUsed == false)
        {
            _greenParticle.Play();
        }
    }

    void TurnOffAvailable()
    {
        if(_isUsed == false)
        {
            _greenParticle.Stop();
        }
    }

    void TurOnForSale()
    {
        if (_isUsed == true)
        {
            _greenParticle.Play();
        }
    }

    void TurOffForSale()
    {
        if (_isUsed == true)
        {
            _greenParticle.Stop();
        }
    }

    public bool GetSpotAvailability()
    {
        return _isUsed;
    }

    public void SetTower(GameObject obj)
    {
        _isUsed = true;
        _spotTower = obj;
        _towerAI = _spotTower.GetComponent<TowerAI>();
        if (_towerAI == null)
            Debug.LogError("TowerAI is NULL on " + transform.name);
        _greenParticle.Stop();
    }

    public void SellTower()
    {
        _isUsed = false;
        _towerAI.Hide();
        _spotTower = null;
        _towerAI = null;
        _greenParticle.Stop();
    }
}
