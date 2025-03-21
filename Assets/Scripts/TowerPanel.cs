using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerPanel : MonoBehaviour
{

    public GameObject statPrefab;
    public GameObject statsField;
    public TextMeshProUGUI towerNameText;
    public GameObject[] switchModeButtons;
    public GameObject upgradeButton;
    public TextMeshProUGUI upgradeText;
    public TextMeshProUGUI levelText;

    private Tower tower = null;
    private Base mainBase;

    private static Color colorOn = new Color(1f, 1f, 1f, 1f);
    private static Color colorOff = new Color(0.37f, 0.37f, 0.37f, 1f);


    void Start()
    {
        mainBase = GameObject.Find("Base").GetComponent<Base>();
        upgradeButton.GetComponent<Button>().onClick.AddListener(upgrade);

        for (int i = 0; i < switchModeButtons.Length; i++)
        {
            int temp = i;
            switchModeButtons[i].GetComponent<Button>().onClick.AddListener(() => switchMode(temp));
        }
        switchMode(0);
    }

    private void Update()
    {
        if (mainBase.getMoney() >= tower.getCost() && !tower.isMax())
        {
            upgradeButton.GetComponent<Image>().color = colorOn;
            upgradeButton.GetComponent<Button>().enabled = true;
        }
        else
        {
            upgradeButton.GetComponent<Image>().color = colorOff;
            upgradeButton.GetComponent<Button>().enabled = false;
        }
        if (tower.isMax())
        {
            //upgradeButton.GetComponent<Image>().color = colorOff;
            //upgradeButton.GetComponent<Button>().enabled = false;
            //upgradeText.gameObject.SetActive(false);
        }
    }


    public void setPanel()
    {
        gameObject.SetActive(true);
        GameObject.Find("Radius").GetComponent<Radius>().createRadius(tower.transform.position, tower.getRadius());
        towerNameText.text = tower.getTowerName();
        upgradeText.text = tower.getCost() + " coins";
        levelText.text = tower.getLevel();

        foreach (Transform child in statsField.transform)
        {
            Destroy(child.gameObject);
        }
        Dictionary<string, float> dict = tower.getStats();
        foreach (string key in dict.Keys)
        {
            GameObject gm = Instantiate(statPrefab, statsField.transform);
            gm.GetComponent<Stat>().setText(key, dict[key].ToString("F1"));
        }

    }

    public void upgrade()
    {
        mainBase.addMoney(-tower.getCost());
        tower.upgrade();
        setPanel();
    }

    public void switchMode(int mode)
    {
        for (int i = 0; i < switchModeButtons.Length; i++)
        {
            if (i == mode)
                switchModeButtons[i].GetComponent<Image>().color = colorOn;
            else
                switchModeButtons[i].GetComponent<Image>().color = colorOff;
        }
        tower.setTargetMode(mode);
    }

    public void setTower(Tower t)
    {
        tower = t;
    }

    public void closePanel()
    {
        GameObject.Find("Radius").GetComponent<Radius>().offRadius();
        gameObject.SetActive(false);
    }

}
