using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemySpawnPanel : MonoBehaviour
{
    public GameObject panel;

    public TextMeshProUGUI enemyCountText;
    public TextMeshProUGUI waveAwardText;
    public TextMeshProUGUI simpleEnemyHealth;
    public TextMeshProUGUI fastEnemyHealth;
    public TextMeshProUGUI strongEnemyHealth;

    private EnemySpawn enemySpawn;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        panel.SetActive(false);
        enemySpawn = GameObject.Find("EnemySpawn").GetComponent<EnemySpawn>();
    }

    void Update()
    {
        enemyCountText.text = "Enemy Count: " + enemySpawn.getEnemyCount();
        waveAwardText.text = "Wave Award: " + enemySpawn.getWaveAward();

        simpleEnemyHealth.text = "HP: " + (enemySpawn.getWavePower() * 60).ToString("F1");
        fastEnemyHealth.text = "HP: " + (enemySpawn.getWavePower() * 35).ToString("F1");
        strongEnemyHealth.text = "HP: " + (enemySpawn.getWavePower() * 115).ToString("F1");
    }

    private void OnMouseUpAsButton()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        GameObject.FindGameObjectWithTag("Platform").GetComponent<Platform>().closePanels();
        panel.SetActive(true);

        GameObject gm = GameObject.Find("Music");
        if (gm) gm.GetComponent<SoundController>().playButtonSound();
    }
    public void closePanel()
    {
        panel.SetActive(false);
    }

    public void closePanelButton()
    {
        panel.SetActive(false);

        GameObject gm = GameObject.Find("Music");
        if (gm) gm.GetComponent<SoundController>().playButtonSound();
    }
}
