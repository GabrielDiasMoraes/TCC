using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPool : MonoBehaviour
{
    public static ColorPool Instance { get; private set; }

    [SerializeField] private List<Color> _availableColors;

    [SerializeField] private Material _sourceMinion;

    [SerializeField] private Material _sourceFragment;
    
    private Dictionary<Color,Material> _poolMaterialsMinions;
    
    private Dictionary<Color,Material> _poolMaterialsFragment;

    public List<Color> AvailableColors => _availableColors;

    private void Awake()
    {
        if (Instance != null) return;
        Instance = this;
        DontDestroyOnLoad(gameObject);
        _poolMaterialsMinions = new Dictionary<Color, Material>();
        _poolMaterialsFragment = new Dictionary<Color, Material>();
    }

    public Material GetMaterial(Color pColor, bool isFragment)
    {
        if (!isFragment)
        {
            if (_poolMaterialsMinions.ContainsKey(pColor)) 
                return _poolMaterialsMinions[pColor];
            Material temp = new Material(_sourceMinion) {color = pColor};
            _poolMaterialsMinions.Add(pColor, temp);

            return _poolMaterialsMinions[pColor];
        }
        else
        {
            if (_poolMaterialsFragment.ContainsKey(pColor)) 
                return _poolMaterialsFragment[pColor];
            Material temp = new Material(_sourceFragment) {color = pColor};
            _poolMaterialsFragment.Add(pColor, temp);

            return _poolMaterialsFragment[pColor];
        }
    }

    
}
