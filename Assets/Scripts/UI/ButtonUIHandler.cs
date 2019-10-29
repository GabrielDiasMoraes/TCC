using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonUIHandler : MonoBehaviour
{
    [SerializeField] private GameObject _panelToActivate;
    
    [SerializeField] private GameObject _panelToDeactivate;


    private void Start()
    {
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(this.ActivatePanel);
    }

    private void ActivatePanel()
    {
        _panelToActivate.SetActive(true);
        _panelToDeactivate.gameObject.SetActive(false);
    }
    
}
