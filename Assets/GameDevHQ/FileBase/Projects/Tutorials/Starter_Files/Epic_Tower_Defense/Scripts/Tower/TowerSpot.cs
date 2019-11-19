using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpot : MonoBehaviour
{
    private ParticleSystem _greenParticle;
    private MeshRenderer _radius;
    private bool _isUsed = false;
    private bool _onMe = false;
    private GameObject _spotTower = null;
    private ITower _towerAI;

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
            if (_onMe == true)
                _radius.enabled = true;
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
            if (_onMe == true)
                _radius.enabled = true;
        }
        else
        {
            _greenParticle.Stop();
            if (_radius.enabled == true)
                _radius.enabled = false;
        }
    }

    void OnMouseEnter()
    {
        _onMe = true;
        TowerPlacement.Instance.SpotCheck(true);        
        bool summoning = TowerPlacement.Instance.GetSumonning();
        bool removing = TowerPlacement.Instance.GetRemoving();
        if ((summoning == true && _isUsed == false) || (removing == true && _isUsed == true))
        {
            _radius.enabled = true;
            TowerPlacement.Instance.StayAtSpotPosition(this.transform.position);
        }
    }

    void OnMouseOver()
    {

    }

    void OnMouseExit()
    {
        _onMe = false;
        _radius.enabled = false;
        TowerPlacement.Instance.SpotCheck(false);
    }

    void OnMouseDown()
    {
        int towerType = TowerPlacement.Instance.GetTowerType();        
        bool summoning = TowerPlacement.Instance.GetSumonning();
        bool removing = TowerPlacement.Instance.GetRemoving();
        if (summoning == true)
        {
            GameObject newTower = SaleManager.Instance.RequestTower(TowerPlacement.Instance.GetTowerType(), this.transform.position);
            ITower iTower = newTower.GetComponent<ITower>();
            if (iTower != null)
            {
                bool availableSpot = GetFundsAvailability(iTower.WarfundCost);
                if(availableSpot == false)
                {
                    GameManager.Instance.RemoveFunds(iTower.WarfundCost);
                    SetTower(newTower);
                }
                else
                {
                    newTower.SetActive(false);
                }
            }
            
        }
        else if (_isUsed == true && removing == true)
        {
            SellTower();
        }
    }

    public bool GetFundsAvailability(int warfundCost)
    {
        if((GameManager.Instance.GetFunds() >= warfundCost) && _isUsed == false)
        {
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
        _spotTower.SetActive(true);
        _towerAI = _spotTower.GetComponent<ITower>();
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
        _radius.enabled = false;
    }
}
