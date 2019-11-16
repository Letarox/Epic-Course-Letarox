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
        TowerPlacement.onAvailable += TurnOnAvailable;
        TowerPlacement.onSale += TurOnSale;
    }

    void OnDisable()
    {
        TowerPlacement.onAvailable -= TurnOnAvailable;
        TowerPlacement.onSale -= TurOnSale;
    }

    void TurnOnAvailable(bool availability)
    {
        if (availability == true && _isUsed == false)
        {
            _greenParticle.Play();
        }
        else
        {
            _greenParticle.Stop();
        }
    }

    void TurOnSale(bool availability)
    {
        if (availability == true && _isUsed == true)
        {
            _greenParticle.Play();
        }
        else
        {
            _greenParticle.Stop();
        }
    }

    public bool GetSpotAvailability(int towerType)
    {
        if((GameManager.Instance.GetFunds() >= SaleManager.Instance.GetTowerCost(towerType)) && _isUsed == false)
        {
            GameManager.Instance.RemoveFunds(SaleManager.Instance.GetTowerCost(towerType));
            return false;
        }
        else
        {
            return true;
        }
        
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
        GameManager.Instance.AddFunds(SaleManager.Instance.GetTowerCost(_towerAI.GetTowerType()));        
        _isUsed = false;
        _towerAI.Hide();
        _spotTower = null;
        _towerAI = null;
        _greenParticle.Stop();
    }
}
