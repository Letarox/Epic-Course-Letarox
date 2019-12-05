using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMananger : MonoSingleton<UIMananger>
{
    [SerializeField]
    private Text _warfundsText;
    [SerializeField]
    public Image _UpgradeGun;
    void Start()
    {
        UpdateWarfunds(GameManager.Instance.GetFunds());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateWarfunds(int warfundsAmount)
    {
        _warfundsText.text = warfundsAmount.ToString();
    }
}
