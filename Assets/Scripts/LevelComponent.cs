using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

public class LevelComponent : MonoBehaviour
{

    [SerializeField] private string levelId;

    [SerializeField] private Image star1;
    
    [SerializeField] private Image star2;
    
    [SerializeField] private Image star3;
    
    
    private Color winColor = new Color(242f/255f, 199f/255f, 68f/255f, 1);
    private Color loseColor = new Color(142f/255f, 142f/255f, 142f/255f, 1);
    
    void Start()
    {
        SetData();
    }

    private void OnEnable()
    {
        SetData();
    }

    void SetData()
    {
        if (string.IsNullOrEmpty(levelId)) return;
        JSONObject niveis = DataController.Instance.Data["niveis"].AsObject;
        JSONObject level = niveis[levelId].AsObject;
        JSONNode count = level["starCount"];
        int starCount = count.AsInt; 
        star1.color = (starCount >= 1) ? winColor : loseColor;
        star2.color = (starCount >= 2) ? winColor : loseColor;
        star3.color = (starCount >= 3) ? winColor : loseColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
