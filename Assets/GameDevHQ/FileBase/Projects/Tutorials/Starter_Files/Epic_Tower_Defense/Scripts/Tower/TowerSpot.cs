using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpot : MonoBehaviour
{
    private ParticleSystem _greenParticle, _redParticle;
    private bool _isUsed = false;
    private GameObject _spotTower = null;

    [System.Obsolete]
    void Start()
    {
        _greenParticle = transform.FindChild("Circle_Selection_Green").GetComponent<ParticleSystem>();
        if (_greenParticle == null)
            Debug.LogError("Green particle is NULL on " + transform.name);

        _redParticle = transform.FindChild("Circle_Selection_Red").GetComponent<ParticleSystem>();
        if (_redParticle == null)
            Debug.LogError("Red particle is NULL on " + transform.name);
    }

    void OnEnable()
    {
        TowerPlacement.onAvailableOn += TurnOnAvailable;
        TowerPlacement.onAvailableOff += TurnOffAvailable;
    }

    void OnDisable()
    {
        TowerPlacement.onAvailableOn -= TurnOnAvailable;
        TowerPlacement.onAvailableOff -= TurnOffAvailable;
    }

    void TurnOnAvailable()
    {
        if(_isUsed == false)
        {
            _greenParticle.Play();
        }
        else
        {
            _redParticle.Play();
        }
    }

    void TurnOffAvailable()
    {
        if(_isUsed == false)
        {
            _greenParticle.Stop();
        }
        else
        {
            _redParticle.Stop();
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
    }

    public void SellTower()
    {
        _isUsed = false;
        if (_spotTower.GetComponent<TowerAI>() != null)
            _spotTower.GetComponent<TowerAI>().Hide();
        _spotTower = null;
        _redParticle.Stop();
        _greenParticle.Play();
    }

    public void TurnColorOnClick()
    {
        _isUsed = true;
        _greenParticle.Stop();
        _redParticle.Play();
    }
}
