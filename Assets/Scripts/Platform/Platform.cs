using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Platform : MonoBehaviour
{
    public GameObject towerPanelPrefab;

    private GameObject towerPanel = null;
    private GameObject platformPanel;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        platformPanel = GameObject.Find("PlatformPanel");
    }

    void Start()
    {
        closeTowerPanel();
        closePlatformPanel();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        closeTowerPanel();
        closePlatformPanel();
        if (towerPanel != null)
        {
            openTowerPanel();
        }
        else
        {
            openPlatformPanel();
        }
    }

    void openTowerPanel()
    {
        closePlatformPanel();
        towerPanel.GetComponent<TowerPanel>().setPanel();
    }
    void closeTowerPanel()
    {
        GameObject[] towerPanels = GameObject.FindGameObjectsWithTag("TowerPanel");
        foreach (GameObject tp in towerPanels)
        {
            tp.GetComponent<TowerPanel>().closePanel();
        }
    }

    void openPlatformPanel()
    {
        platformPanel.GetComponent<PlatformPanel>().setPanel(this);
    }

    void closePlatformPanel()
    {
        platformPanel.GetComponent<PlatformPanel>().closePanel();
    }
    
    public void createTowerPanel(GameObject t)
    {
        towerPanel = Instantiate(towerPanelPrefab, GameObject.Find("Canvas").transform);
        towerPanel.GetComponent<TowerPanel>().setTower(t.GetComponent<Tower>());
        openTowerPanel();
    }

    
}
