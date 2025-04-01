using TMPro;
using UnityEngine;

public class Base : MonoBehaviour
{

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI waveUnderText;

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
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy e = collision.gameObject.GetComponent<Enemy>();
            health -= e.getPower();
            GameObject.Find("EnemySpawn").GetComponent<EnemySpawn>().offAward(e.getWaveNum());
            Destroy(collision.gameObject);
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

    public void addMoney(int m)
    {
        money += m;
        updateMoneyText();
    }

}
