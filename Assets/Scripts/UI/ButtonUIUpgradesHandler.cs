using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonUIUpgradesHandler : MonoBehaviour
{

    [Header("Components")]
    [SerializeField] UpgradeController _upgradeController;

    [SerializeField] private TextMeshProUGUI _title;
    
    [SerializeField] private TextMeshProUGUI _desc;
    
    [SerializeField] private Button _btnBuy;

    [SerializeField] private TextMeshProUGUI _btnText;

    [SerializeField] private GameObject highlight;
    
    [Header("Values")]
    [SerializeField] private string _titleValue;
    
    [SerializeField] private string _descValue;
    
    [SerializeField] private int _price;

    [SerializeField] private AbilityTypes abilityType;

    [SerializeField] private AbilityTypes needsToUnlock;
    
    
    void Start()
    {
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(delegate
        {
            if (needsToUnlock != AbilityTypes.None)
            {
                bool bIsInteractable = (DataController.Instance.OwnedAbilities.Contains(needsToUnlock));
                bIsInteractable = (bIsInteractable)? !DataController.Instance.OwnedAbilities.Contains(abilityType): bIsInteractable;
                _btnBuy.interactable = bIsInteractable;
            }
            else
            {
                _btnBuy.interactable = !DataController.Instance.OwnedAbilities.Contains(abilityType);
            }
            _title.text = _titleValue;
            _desc.text = _descValue;
            _btnText.text = _price.ToString();
            _upgradeController.ChangeHighlight(highlight);
            _btnBuy.onClick.RemoveAllListeners();
            _btnBuy.onClick.AddListener(delegate
            {
                _upgradeController.BuyAbility(_price, abilityType);
                _btnBuy.interactable = false;
            });
        });
    }


}
