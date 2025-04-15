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
    public GameObject sellButton;
    public TextMeshProUGUI sellText;
    public TextMeshProUGUI levelText;

    private Tower tower = null;
    private Base mainBase;

    private static Color colorOn = new Color(1f, 1f, 1f, 1f);
    private static Color colorOff = new Color(0.37f, 0.37f, 0.37f, 1f);


    void Awake()
    {
        mainBase = GameObject.Find("Base").GetComponent<Base>();
        upgradeButton.GetComponent<Button>().onClick.AddListener(upgrade);
        sellButton.GetComponent<Button>().onClick.AddListener(sell);

        for (int i = 0; i < switchModeButtons.Length; i++)
        {
            int temp = i;
            switchModeButtons[i].GetComponent<Button>().onClick.AddListener(() => switchMode(temp));
        }
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
            upgradeButton.GetComponent<Image>().color = colorOff;
            upgradeButton.GetComponent<Button>().enabled = false;
            upgradeText.text = "Max level";
        }
    }


    public void setPanel()
    {
        gameObject.SetActive(true);
        GameObject.Find("Radius").GetComponent<Radius>().createRadius(tower.transform.position, tower.getRadius());
        towerNameText.text = tower.getTowerName();
        upgradeText.text = tower.getCost() + " coins";
        sellText.text = tower.getSellCost() + " coins";
        levelText.text = tower.getLevelStr();

        foreach (Transform child in statsField.transform)
        {
            Destroy(child.gameObject);
        }
        Dictionary<string, string> dict = tower.getStats();
        foreach (string key in dict.Keys)
        {
            GameObject gm = Instantiate(statPrefab, statsField.transform);
            gm.GetComponent<Stat>().setText(key, dict[key]);
        }

    }

    public void upgrade()
    {
        mainBase.addMoney(-tower.getCost());
        tower.upgrade();
        setPanel();
    }

    public void sell()
    {
        mainBase.addMoney(tower.getSellCost());
        closePanel();
        Destroy(tower.gameObject);
        Destroy(gameObject);
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
        mainBase.addMoney(-tower.getCost());
        tower.buy();
        switchMode(tower.getTargetMode());
    }

    public void closePanel()
    {
        GameObject.Find("Radius").GetComponent<Radius>().offRadius();
        gameObject.SetActive(false);
    }

}
