using System.Collections.Generic;
using System.Text;
using UnityEngine;
using SimpleJSON;

public class DataController : MonoBehaviour
{

    public static DataController Instance { get; private set; }

    private string currentLevel;

    private int _currentPoints;
    
    private JSONNode data;

    private int currentDifficulty;

    private List<AbilityTypes> ownedAbilities;

    private bool? _hasIntelligenceBonus;
    
    private bool? _hasMinionAddition;
    
    private bool? _hasShieldAbility;

    #region Properties

    public JSONNode Data
    {
        get => data;
        set => data = value;
    }

    public string CurrentLevel
    {
        get => currentLevel;
        set => currentLevel = value;
    }

    public int CurrentPoints
    {
        get => _currentPoints;
        set
        {
            _currentPoints = value;
            data["pontuacao"] = value;
        }
    }

    public int CurrentDifficulty
    {
        get => currentDifficulty;
        set
        {
            currentDifficulty = value;
            data["config"]["dificuldade"] = value;
        }
    }

    public List<AbilityTypes> OwnedAbilities
    {
        get => ownedAbilities;
        set => ownedAbilities = value;
    }

    public bool? HasIntelligenceBonus => _hasIntelligenceBonus ?? (_hasIntelligenceBonus = ownedAbilities.Contains(AbilityTypes.Smarter));

    public bool? HasShieldAbility => _hasShieldAbility ?? (_hasShieldAbility = ownedAbilities.Contains(AbilityTypes.Shield));
    
    public bool? HasMinionAddition => _hasMinionAddition ?? (_hasMinionAddition = ownedAbilities.Contains(AbilityTypes.More_Minions));

    #endregion
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        _hasIntelligenceBonus = null;

    }

    private void Start()
    {
        LoadData();
        if (data.Count == 0)
        {
            InitializeData();
        }

        _currentPoints = data["pontuacao"];
        currentDifficulty = data["config"]["dificuldade"].AsInt;
    }

    private void InitializeData()
    {
        StringBuilder builder = new StringBuilder("{");
        builder.Append("\"config\": { \"dificuldade\": \"1\" },");
        builder.Append("\"melhorias\": [],");
        builder.Append("\"niveis\": {");
        builder.Append("\"Nvl1\":{\"starCount\" : \"0\"},},");
        builder.Append("\"pontuacao\": \"30\"");
        builder.Append("}");

        string stringField = builder.ToString();

        data = JSON.Parse(stringField);

        SaveData(data.ToString());
        
    }

    public void SaveData(string jsonData = null)
    {
        if (string.IsNullOrEmpty(jsonData))
        {
            jsonData = data.ToString();
        }
        PlayerPrefs.SetString("data", jsonData);
        PlayerPrefs.Save();
    }

    public void LoadData()
    {
        data = JSON.Parse(GetData());
        if(data.Count == 0) return;
        ownedAbilities = AbilitiesJsonToList();
    }

    private List<AbilityTypes> AbilitiesJsonToList()
    {
        List<AbilityTypes> list = new List<AbilityTypes>();

        JSONArray melhorias = data["melhorias"].AsArray;
        for (int i = 0; i < melhorias.Count; i++)
        {
            AbilityTypes tempType = (AbilityTypes) melhorias[i].AsInt;
            list.Add(tempType);
        }
        
        return list;
    }


    public void AddAbility(AbilityTypes type)
    {
        if (ownedAbilities != null) {}
        else
        {
            ownedAbilities = new List<AbilityTypes>();
        }
        if(ownedAbilities.Contains(type)) {}
        else { ownedAbilities.Add(type); }
        data["melhorias"].Add((int)type);
        SaveData();
        LoadData();
    }

    public string GetData()
    {
        return PlayerPrefs.GetString("data", "");
    }
}
