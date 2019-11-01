using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointComponent : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI pointsQuantity;
    
    void Start()
    {
        SetData();
    }

    private void OnEnable()
    {
        SetData();
    }

    public void SetData()
    {
        pointsQuantity.text = DataController.Instance.CurrentPoints.ToString();
    }

}
