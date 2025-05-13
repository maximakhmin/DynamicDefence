using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Base : MonoBehaviour
{

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI waveUnderText;

    public GameObject endPanel;
    public TextMeshProUGUI endPanelNameText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI recordText;


    private int health = 20;
    private int money = 80;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LineRenderer line = GameObject.Find("MainPath").GetComponent<LineRenderer>();
        transform.position = line.GetPosition(line.positionCount - 1) + new Vector3(0, 0, -5);

        updateHealthText();
        updateMoneyText();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Screen.width + " " + Screen.height);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy e = collision.gameObject.GetComponent<Enemy>();
            health -= e.getPower();

            if (health <= 0)
            {
                endLevel();
            }

            GameObject.Find("EnemySpawn").GetComponent<EnemySpawn>().offAward(e.getWaveNum());
            GameObject.Find("MLDDA").GetComponent<MLDDA>().addLastHitPosition(transform.position);
            Destroy(collision.gameObject);
            GameObject gm = GameObject.Find("Music");
            if (gm) gm.GetComponent<SoundController>().playBaseDamageSound();
            updateHealthText();
        }
    }

    private void updateHealthText()
    {
        healthText.text = "HP " + health;
    }

    private void updateMoneyText()
    {
        moneyText.text = "Coins " + money;
    }
    public void updateWaveText(string text)
    {
        waveText.text = "" + text;
    }
    public void updateWaveUnderText(string text)
    {
        waveUnderText.text = "" + text;
    }

    public int getMoney()
    {
        return money;
    }

    public int getHealth()
    {
        return health;
    }

    public void addMoney(int m)
    {
        money += m;
        updateMoneyText();
    }

    public void endLevel()
    {
        //GameObject.Find("MLPlayer").GetComponent<MLPlayer>().endEpisodeVoid(b);
        GameObject.Find("MLDDA").GetComponent<MLDDA>().endEpisodeVoid();
        //train 
        /*
        int a = Random.Range(0, 3);
        SceneManager.LoadScene(a);
        */
        Time.timeScale = 0f;


        string levelName = SceneManager.GetActiveScene().name;
        int score = GameObject.Find("EnemySpawn").GetComponent<EnemySpawn>().getWaveNum();

        endPanelNameText.text = levelName;
        scoreText.text = "Your score: " + score;

        int record = DataBaseAdapter.getLevelRecord(levelName);

        if (score > record)
        {
            recordText.text = "That is your new record";
            DataBaseAdapter.setLevelRecord(levelName, score);
        }
        else
        {
            recordText.text = "Record: " + record;
        }

        GameObject.FindGameObjectWithTag("Platform").GetComponent<Platform>().closePanels();
        GameObject.Find("EnemySpawn").GetComponent<EnemySpawnPanel>().closePanel();

        endPanel.SetActive(true);
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        GameObject gm = GameObject.Find("Music");
        if (gm) gm.GetComponent<SoundController>().playButtonSound();
    }

    public void menu()
    {
        SceneManager.LoadScene("Menu");

        GameObject gm = GameObject.Find("Music");
        if (gm) gm.GetComponent<SoundController>().playButtonSound();
    }

}
