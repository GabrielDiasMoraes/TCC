using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigComponent : MonoBehaviour
{
    private int currentDifficulty;
    private int newDifficulty;

    [SerializeField] private GameObject[] highlights;

    [SerializeField] private Button saveButton;
    
    [SerializeField] private Button loadButton;

    private void SetData()
    {
        newDifficulty = currentDifficulty = DataController.Instance.CurrentDifficulty;
        foreach (var highlight in highlights)
        {
            highlight.SetActive(false);
        }

        highlights[currentDifficulty].SetActive(true);
    }
    
    private void Start()
    {
        SetData();
        
        saveButton.onClick.AddListener(delegate
        {
            DataController.Instance.CurrentDifficulty = currentDifficulty = newDifficulty;
            DataController.Instance.SaveData();
        });
        
        loadButton.onClick.AddListener(delegate
        {
            DataController.Instance.LoadData();
            SetData();
        });
    }

    private void OnEnable()
    {
        SetData();
    }

    public void SetDifficulty(int pNewDifficulty)
    {
        highlights[currentDifficulty].SetActive(false);
        highlights[pNewDifficulty].SetActive(true);
        newDifficulty = pNewDifficulty;
    }
}
