using UnityEngine;
using UnityEngine.UI;

public class PlatformPanel : MonoBehaviour
{

    public GameObject[] towerPrefabs;
    public Button[] towerPrefabButtons;

    private Platform activePlatform;
    private Base mainBase;
    private static int[] costs = new[] { 30, 50, 70, 80 };
    private GrayScale[] grayScales;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainBase = GameObject.Find("Base").GetComponent<Base>();

        for (int i = 0; i < towerPrefabButtons.Length; i++)
        {
            int num = i;
            towerPrefabButtons[i].onClick.RemoveAllListeners();
            towerPrefabButtons[i].onClick.AddListener(() => buyTower(num));
        }
        gameObject.SetActive(false);

        grayScales = new GrayScale[costs.Length];

        for (int i = 0; i < costs.Length; i++)
        {
            grayScales[i] = towerPrefabButtons[i].transform.GetChild(0).GetComponent<GrayScale>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < costs.Length; i++)
        {
            if (mainBase.getMoney() < costs[i])
            {
                grayScales[i].setGray();
                towerPrefabButtons[i].enabled = false;
            }
            else
            {
                grayScales[i].setColor();
                towerPrefabButtons[i].enabled = true;
            }
        }
    }

    public void setPanel(Platform pl)
    {
        activePlatform = pl;
        gameObject.SetActive(true);
    }

    void buyTower(int num)
    {
        GameObject gm = Instantiate(towerPrefabs[num], activePlatform.transform);
        activePlatform.createTowerPanel(gm);
    }

    public void closePanel()
    {
        gameObject.SetActive(false);
    }



}
