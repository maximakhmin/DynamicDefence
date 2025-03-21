using UnityEngine;
using UnityEngine.UI;

public class PlatformPanel : MonoBehaviour
{

    public GameObject[] towerPrefabs;
    public Button[] towerPrefabButtons;

    private Platform activePlatform;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setPanel(Platform pl)
    {
        activePlatform = pl;
        for (int i = 0; i < towerPrefabButtons.Length; i++)
        {
            int num = i;
            towerPrefabButtons[i].onClick.RemoveAllListeners();
            towerPrefabButtons[i].onClick.AddListener(() => buyTower(num));
        }
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
