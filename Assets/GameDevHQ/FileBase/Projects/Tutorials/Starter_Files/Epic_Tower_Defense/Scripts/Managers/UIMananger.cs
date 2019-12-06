using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMananger : MonoSingleton<UIMananger>
{
    [SerializeField]
    private Text _warfundsText;
    [SerializeField]
    private GameObject[] _weapons;
    [SerializeField]
    private GameObject[] _removeWeapons;
    [SerializeField]
    private GameObject _dismantleWeapon;
    [SerializeField]
    private GameObject _upgradeDualGattlingGun, _upgradeDualMissileTurret;
    [SerializeField]
    private GameObject _upgradeGattling, _upgradeMissile;

    private int _currentTowerWorth;
    private GameObject _currentSpot;

    private GameObject _currentDisplay;

    private Button _dualGattlingButton, _dualMissileTurretButton;

    void OnEnable()
    {
        TowerSpot.OnSpotClick += UpdateTowerInformationDisplay;
        TowerPlacement.OnCancel += ResetInformationDisplay;
    }

    void OnDisable()
    {
        TowerSpot.OnSpotClick -= UpdateTowerInformationDisplay;
        TowerPlacement.OnCancel -= ResetInformationDisplay;
    }

    [System.Obsolete]
    void Start()
    {
        UpdateWarfunds(GameManager.Instance.GetFunds());
        _dualGattlingButton = _upgradeGattling.transform.FindChild("Accept").GetComponent<Button>();
        _dualMissileTurretButton = _upgradeMissile.transform.FindChild("Accept").GetComponent<Button>();

        if (_dualGattlingButton == null)
            Debug.LogError("Dual Gattling Button is NULL on " + transform.name);

        if (_dualMissileTurretButton == null)
            Debug.LogError("Dual Missile Turret Button is NULL on " + transform.name);

        _dualGattlingButton.onClick.AddListener(OnClickUpgrade);
        _dualMissileTurretButton.onClick.AddListener(OnClickUpgrade);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateWarfunds(int warfundsAmount)
    {
        _warfundsText.text = warfundsAmount.ToString();
    }

    public void ResetInformationDisplay()
    {
        HideAllImages();
        _dismantleWeapon.SetActive(false);
        _upgradeDualGattlingGun.SetActive(false);
        _upgradeDualMissileTurret.SetActive(false);
        _upgradeGattling.SetActive(false);
        _upgradeMissile.SetActive(false);
        _weapons[0].SetActive(true);
        _weapons[1].SetActive(true);
    }

    public void UpdateTowerInformationDisplay(int towerType, int towerCost, GameObject spot)
    {
        _currentTowerWorth = towerCost;
        _currentSpot = spot;
        switch (towerType)
        {
            case 0:
                HideAllImages();
                _weapons[2].SetActive(true);
                _removeWeapons[0].SetActive(true);
                break;
            case 1:
                HideAllImages();
                _weapons[3].SetActive(true);
                _removeWeapons[1].SetActive(true);
                break;
            case 2:
                HideAllImages();
                _removeWeapons[2].SetActive(true);
                break;
            case 3:
                HideAllImages();
                _removeWeapons[3].SetActive(true);
                break;
            default:                
                break;
        }
    }

    void HideAllImages()
    {
        for(int i = 0; i < _weapons.Length; i++)
        {
            _weapons[i].SetActive(false);
        }
        for (int x = 0; x < _removeWeapons.Length; x++)
        {
            _removeWeapons[x].SetActive(false);
        }
    }

    public void DismantleWeapon(bool isRemoving)
    {
        if (_currentDisplay != null)
            _currentDisplay.SetActive(false);
        if (isRemoving == true)
        {
            _currentDisplay = _dismantleWeapon;
            _currentDisplay.SetActive(true);
            _dismantleWeapon.GetComponentInChildren<Text>().text = _currentTowerWorth.ToString();
        }
        else
        {
            _dismantleWeapon.SetActive(false);
        }
    }

    public void AcceptSellingTower()
    {
        TowerSpot spotScript = _currentSpot.GetComponent<TowerSpot>();
        if (spotScript != null)
            spotScript.SellTower();

        _dismantleWeapon.SetActive(false);
        ResetInformationDisplay();
    }

    public void DeclineSellingTower()
    {
        _dismantleWeapon.SetActive(false);
    }

    public void DisplayGattlingGunUpgrade()
    {
        if (_currentDisplay != null)
            _currentDisplay.SetActive(false);
        _currentDisplay = _upgradeDualGattlingGun;
        _currentDisplay.SetActive(true);
       /* if (_upgradeGattling.activeInHierarchy == false)
            _upgradeDualGattlingGun.SetActive(true);
        if (_upgradeMissile.activeInHierarchy == true)
            _upgradeDualMissileTurret.SetActive(false);*/
    }

    public void DisplayMissileTurretUpgrade()
    {
        if (_currentDisplay != null)
            _currentDisplay.SetActive(false);
        _currentDisplay = _upgradeDualMissileTurret;
        _currentDisplay.SetActive(true);
    }

    public void DeclineUpgrade()
    {
        _currentDisplay.SetActive(false);
    }

    void OnClickUpgrade()
    {
        TowerSpot spotScript = _currentSpot.GetComponent<TowerSpot>();
        if (spotScript != null)
            spotScript.UpgradeTower();
        if (_upgradeGattling.activeInHierarchy == true)
        {
            _currentTowerWorth = 320;
            _upgradeDualGattlingGun.SetActive(false);
            HideAllImages();
            _removeWeapons[2].SetActive(true);
        }
            
        if (_upgradeMissile.activeInHierarchy == true)
        {
            _currentTowerWorth = 400;
            _upgradeDualMissileTurret.SetActive(false);
            HideAllImages();
            _removeWeapons[3].SetActive(true);
        }
            
    }
}
