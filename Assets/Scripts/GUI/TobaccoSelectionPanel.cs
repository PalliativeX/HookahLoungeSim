using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class TobaccoSelectionPanel : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public TMP_Text label;
    public TMP_Text description;
    Player player;

    int currentValue;
    List<Tobacco> tobaccos;

    public Tobacco FinallyChosenTobacco { get; set; }

    void Start()
    {
        player = FindObjectOfType<Player>();
        tobaccos = player.tobaccos;
        InitDropdown();
        OnValueChanged();
        UpdateDescription();
    }

    void InitDropdown()
    {
        dropdown.options.Clear();

        foreach (Tobacco tobacco in tobaccos)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData(tobacco.NameStr()));
        }
        dropdown.value = currentValue;
        dropdown.options[currentValue].text = tobaccos[currentValue].NameStr(); 
    }

    void UpdateDescription()
    {
        description.text = tobaccos[currentValue].ToString();
    }

    public void OnValueChanged()
    {
        currentValue = dropdown.value;
        label.text = tobaccos[dropdown.value].NameStr();
        UpdateDescription();
    }

    public void ChooseTobacco()
    {
        FinallyChosenTobacco = tobaccos[currentValue];
    }
}
