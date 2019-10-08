using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnController : MonoBehaviour
{

    [SerializeField] private Dropdown _fitnessTypeSelector;
    [SerializeField] private Slider _elitismTopSlider;
    [SerializeField] private TextMeshProUGUI _elitismTopValue;
    [SerializeField] private Slider _elitismBotSlider;
    [SerializeField] private TextMeshProUGUI _elitismBotValue;
    [SerializeField] private Button _confirmButton;
    
    void Start()
    {
        _elitismTopSlider.minValue = _elitismBotSlider.minValue = PopulationController.Instance.MinimumElitismPercent;
        _elitismBotValue.text = PopulationController.Instance.MinimumElitismPercent.ToString() + "%";
        _elitismTopValue.text = PopulationController.Instance.MinimumElitismPercent.ToString() + "%";
        _elitismTopSlider.maxValue = _elitismBotSlider.maxValue = PopulationController.Instance.MaximumElitismPercent;
        _elitismBotSlider.onValueChanged.AddListener(delegate(float value) { _elitismBotValue.text = value.ToString() + "%"; });
        _elitismTopSlider.onValueChanged.AddListener(delegate(float value) { _elitismTopValue.text = value.ToString() + "%"; });
        _fitnessTypeSelector.options.Clear();
        
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        options.Add(new Dropdown.OptionData(FitnessTypes.DISTANCE.ToString()));
        options.Add(new Dropdown.OptionData(FitnessTypes.SPEED.ToString()));
        options.Add(new Dropdown.OptionData(FitnessTypes.DEFENSE.ToString()));
        options.Add(new Dropdown.OptionData(FitnessTypes.SMARTER.ToString()));
        options.Add(new Dropdown.OptionData(FitnessTypes.SPEED_SMARTER.ToString()));
        options.Add(new Dropdown.OptionData(FitnessTypes.DEFENSE_SMARTER.ToString()));
        
        _fitnessTypeSelector.options.AddRange(options);
        _fitnessTypeSelector.value = 0;
        
        _confirmButton.onClick.AddListener(SetDataInController);
    }

    void SetDataInController()
    {
        PopulationController.Instance.ElitismTopTierPercent = _elitismTopSlider.value;
        PopulationController.Instance.ElitismBottomTierPercent = _elitismBotSlider.value;
        FitnessTypes.TryParse(_fitnessTypeSelector.options[_fitnessTypeSelector.value].text, true, out FitnessTypes type);
        PopulationController.Instance.FitnessType = type;
        PopulationController.Instance.EndTurn();
        this.gameObject.SetActive(false);
    }
}
