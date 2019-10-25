using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI _minionsCount;

    [SerializeField] private TextMeshProUGUI _minionsCountToWin;
    
    [SerializeField] private TextMeshProUGUI _turnsTextMeshProUgui;

    [SerializeField] private GameObject _pausePanel;

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
            DontDestroyOnLoad(gameObject);
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

    public void PauseGame()
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


    }
    
    
}
