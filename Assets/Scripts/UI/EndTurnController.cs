using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnController : MonoBehaviour
{

    [SerializeField] private TMP_Dropdown _fitnessTypeSelector;
    [SerializeField] private Slider _elitismTopSlider;
    [SerializeField] private TextMeshProUGUI _elitismTopValue;
    [SerializeField] private Slider _elitismBotSlider;
    [SerializeField] private TextMeshProUGUI _elitismBotValue;
    [SerializeField] private Button _confirmButton;

    [Header("Right Panel")] 
    [SerializeField] private TextMeshProUGUI _bestLife;
    [SerializeField] private TextMeshProUGUI _bestArmor;
    [SerializeField] private TextMeshProUGUI _bestSpeed;
    [SerializeField] private TextMeshProUGUI _bestIntelligence;
    
    void Start()
    {
        _elitismTopSlider.minValue = _elitismBotSlider.minValue = PopulationController.Instance.MinimumElitismPercent;
        _elitismBotValue.text = PopulationController.Instance.MinimumElitismPercent.ToString() + "%";
        _elitismTopValue.text = PopulationController.Instance.MinimumElitismPercent.ToString() + "%";
        _elitismTopSlider.maxValue = _elitismBotSlider.maxValue = PopulationController.Instance.MaximumElitismPercent;
        _elitismBotSlider.onValueChanged.AddListener(delegate(float value) { _elitismBotValue.text = value.ToString() + "%"; });
        _elitismTopSlider.onValueChanged.AddListener(delegate(float value) { _elitismTopValue.text = value.ToString() + "%"; });
        _fitnessTypeSelector.options.Clear();
        
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        options.Add(new TMP_Dropdown.OptionData(GameUtility.FitnessToString(FitnessTypes.Distance)));
        options.Add(new TMP_Dropdown.OptionData(GameUtility.FitnessToString(FitnessTypes.Speed)));
        options.Add(new TMP_Dropdown.OptionData(GameUtility.FitnessToString(FitnessTypes.Defense)));
        options.Add(new TMP_Dropdown.OptionData(GameUtility.FitnessToString(FitnessTypes.Smarter)));
        options.Add(new TMP_Dropdown.OptionData(GameUtility.FitnessToString(FitnessTypes.SpeedSmarter)));
        options.Add(new TMP_Dropdown.OptionData(GameUtility.FitnessToString(FitnessTypes.DefenseSmarter)));
        
        _fitnessTypeSelector.options.AddRange(options);
        _fitnessTypeSelector.value = 0;
        
        _confirmButton.onClick.AddListener(SetDataInController);

        _bestLife.text = PopulationController.Instance.BestLife.ToString("0.00");
        _bestArmor.text = PopulationController.Instance.BestDefense.ToString("0.00");
        _bestSpeed.text = PopulationController.Instance.BestSpeed.ToString("0.00");
        _bestIntelligence.text = PopulationController.Instance.BestIntelligence.ToString("0.00") + "%";
    }

    void SetDataInController()
    {
        PopulationController.Instance.ElitismTopTierPercent = _elitismTopSlider.value;
        PopulationController.Instance.ElitismBottomTierPercent = _elitismBotSlider.value;
        FitnessTypes type = GameUtility.StringToFitness(_fitnessTypeSelector.options[_fitnessTypeSelector.value].text);
        Debug.Log(type);
        PopulationController.Instance.FitnessType = type;
        PopulationController.Instance.EndTurn();
        this.gameObject.SetActive(false);
    }
}
