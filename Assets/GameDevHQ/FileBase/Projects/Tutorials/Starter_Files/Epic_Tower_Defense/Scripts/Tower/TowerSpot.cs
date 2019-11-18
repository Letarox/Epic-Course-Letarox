using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpot : MonoBehaviour
{
    private ParticleSystem _greenParticle;
    private MeshRenderer _radius;
    private bool _isUsed = false;
    private GameObject _spotTower = null;
    private TowerAI _towerAI;
    private Vector3 _myPos;

    [System.Obsolete]
    void Start()
    {
        _greenParticle = GetComponentInChildren<ParticleSystem>();
        if (_greenParticle == null)
            Debug.LogError("Green particle is NULL on " + transform.name);
        _radius = this.transform.FindChild("Radius").GetComponent<MeshRenderer>();
        if (_radius == null)
            Debug.LogError("Radius is NULL on " + transform.name);
    }

    void OnEnable()
    {
        TowerPlacement.OnAvailable += TurnOnAvailable;
        TowerPlacement.OnSale += TurOnSale;
    }

    void OnDisable()
    {
        TowerPlacement.OnAvailable -= TurnOnAvailable;
        TowerPlacement.OnSale -= TurOnSale;
    }

    void TurnOnAvailable(bool active)
    {
        if (active == true && _isUsed == false)
        {
            _greenParticle.Play();
        }
        else
        {
            _greenParticle.Stop();
            if (_radius.enabled == true)
                _radius.enabled = false;
        }
    }

    void TurOnSale(bool active)
    {
        if (active == true && _isUsed == true)
        {
            _greenParticle.Play();
            if (_radius.enabled == true)
                _radius.enabled = false;
        }
        else
        {
            _greenParticle.Stop();            
        }
    }

    void OnMouseEnter()
    {
        TowerPlacement.Instance.SpotCheck(true);        
        bool summoning = TowerPlacement.Instance.GetSumonning();
        bool removing = TowerPlacement.Instance.GetRemoving();
        _myPos = transform.position;
        if ((summoning == true && _isUsed == false) || (removing == true && _isUsed == true))
        {
            _radius.enabled = true;
            TowerPlacement.Instance.StayAtSpotPosition(this.transform.position);
            Debug.Log("multiple calls...");
        }
    }

    void OnMouseOver()
    {

    }

    void OnMouseExit()
    {
        _radius.enabled = false;
        TowerPlacement.Instance.SpotCheck(false);
    }

    void OnMouseDown()
    {
        Debug.Log("mouse down...");
        int towerType = TowerPlacement.Instance.GetTowerType();
        bool availableSpot = GetFundsAvailability(towerType);
        if (availableSpot == false)
        {
            GameObject newTower = SaleManager.Instance.RequestTower(TowerPlacement.Instance.GetTowerType(), this.transform.position);
            SetTower(newTower);
        }
        else if (_isUsed == true && TowerPlacement.Instance.GetRemoving() == true)
        {
            SellTower();
        }
    }

    public bool GetFundsAvailability(int towerType)
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
