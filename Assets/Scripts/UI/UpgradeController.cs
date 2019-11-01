using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeController : MonoBehaviour
{

    [SerializeField] private PointComponent points;

    private GameObject actualHightlighted;


    public void BuyAbility(int price, AbilityTypes abilityType)
    {
        if (price > DataController.Instance.CurrentPoints) return;
        
        if(DataController.Instance.OwnedAbilities.Contains(abilityType)) return;

        DataController.Instance.CurrentPoints -= price;
        DataController.Instance.AddAbility(abilityType);
        points.SetData();

        
    }

    public void ChangeHighlight(GameObject newHighlight)
    {
        if(actualHightlighted != null)
            actualHightlighted.SetActive(false);
        actualHightlighted = newHighlight;
        actualHightlighted.SetActive(true);
    }
}
