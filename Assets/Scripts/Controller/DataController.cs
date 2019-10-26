using System.Text;
using UnityEngine;
using SimpleJSON;

public class DataController : MonoBehaviour
{

    public static DataController Instance { get; private set; }

    private JSONNode data;

    #region Properties

    public JSONNode Data
    {
        get => data;
        set => data = value;
    }

    #endregion
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        
    }

    private void Start()
    {
        data = JSON.Parse(GetData());
        if (data.Count == 0)
        {
            InitializeData();
        }
    }

    private void InitializeData()
    {
        StringBuilder builder = new StringBuilder("{");
        builder.Append("\"config\": { \"dificuldade\": \"1\" },");
        builder.Append("\"melhorias\": [],");
        builder.Append("\"niveis\": [");
        builder.Append("{\"0\": {\"pontuacao\": \"0\", \"oozes\": [], \"turno\": \"0\"}}]");
        builder.Append("}");

        string stringField = builder.ToString();

        data = JSON.Parse(stringField);

        SaveData(data.ToString());
        
    }

    public void SaveData(string jsonData)
    {
        PlayerPrefs.SetString("data", jsonData);
        PlayerPrefs.Save();
    }

    public string GetData()
    {
        return PlayerPrefs.GetString("data", "");
    }
}
