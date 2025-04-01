using TMPro;
using UnityEngine;

public class Stat : MonoBehaviour
{
    public TextMeshProUGUI statName;
    public TextMeshProUGUI stat;

    public void setText(string statNameText, string statText)
    {
        statName.text = statNameText;
        stat.text = statText;
    }

}
