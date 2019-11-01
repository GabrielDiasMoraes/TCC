using SimpleJSON;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private Color winColor = new Color(242f/255f, 199f/255f, 68/255f, 1);
    private Color loseColor = new Color(240/255f, 81f/255f, 93f/255f, 1);
    public static GameController Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI _minionsCount;

    [SerializeField] private TextMeshProUGUI _minionsCountToWin;
    
    [SerializeField] private TextMeshProUGUI _turnsTextMeshProUgui;

    [SerializeField] private GameObject _pausePanel;

    [SerializeField] private TextMeshProUGUI _endTurnTitle;

    [SerializeField] private GameObject _endTurnPanel;
    
    [SerializeField] private Image _endTurnStar1;
    
    [SerializeField] private Image _endTurnStar2;
    
    [SerializeField] private Image _endTurnStar3;

    private float saveTimeScale;
    
    public TextMeshProUGUI MinionsCountToWin => _minionsCountToWin;

    public TextMeshProUGUI TurnsTextMeshProUgui => _turnsTextMeshProUgui;

    private Camera _mainCamera;

    public Camera MainCamera => _mainCamera;

    private bool isFastmode, isPaused;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }

        isFastmode = false;
    }

    private void Start()
    {
        _mainCamera = Camera.main;
        isPaused = false;
    }

    public void SpeedUpGame()
    {
        if (!isFastmode)
            Time.timeScale = 10f;
        else
            Time.timeScale = 1f;
        isFastmode = !isFastmode;
    }

    public void PauseGame(bool bForcePause = false)
    {
        if (isPaused)
        {
            Time.timeScale = saveTimeScale;
            _pausePanel.SetActive(false);
        }
        else
        {
            _pausePanel.SetActive(true);
            saveTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }

        isPaused = !isPaused;

        if (!bForcePause) return;
        else
        {
            _pausePanel.SetActive(true);
            saveTimeScale = (isPaused)? saveTimeScale : Time.timeScale;
            Time.timeScale = 0;
            isPaused = true;
        }

    }

    public void SetEndTurn(int finalTurn, int necessaryStar1, int necessaryStar2, int necessaryStar3, int maxTurn)
    {
        _endTurnTitle.text = (finalTurn >= maxTurn) ? "Derrota" : "Vitória";
        int starCount = 0;
        starCount += (maxTurn - finalTurn >= necessaryStar1) ? 1 : 0;
        starCount += (maxTurn - finalTurn >= necessaryStar2) ? 1 : 0;
        starCount += (maxTurn - finalTurn >= necessaryStar3) ? 1 : 0;
        _endTurnStar1.color = (starCount >= 1) ? winColor : loseColor;
        _endTurnStar2.color = (starCount >= 2) ? winColor : loseColor;
        _endTurnStar3.color = (starCount >= 3) ? winColor : loseColor;
        _endTurnPanel.SetActive(true);

        
        
        string currentLevel = DataController.Instance.CurrentLevel;

        JSONObject niveis = DataController.Instance.Data["niveis"].AsObject;
        JSONObject level = niveis[currentLevel].AsObject;
        int oldStarCount = level["starCount"].AsInt;

        if (oldStarCount >= starCount) return;

        int actualPoints = DataController.Instance.CurrentPoints;
        
        DataController.Instance.CurrentPoints += maxTurn - finalTurn;
        DataController.Instance.Data["niveis"][currentLevel]["starCount"] = starCount;
        DataController.Instance.SaveData();

    }
    
}
