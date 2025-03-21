using System.Collections;
using System.Drawing;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject HpBar;
    private float HpBarScale;

    public float speed = 100;
    public int power = 1;
    public int award = 1;

    private float health = 100;
    private float maxHealth = 100;
    private float timeHpBar = 3f;
    private float deltaHpBar = 0f;

    private LineRenderer line;
    private int posInd = 0;
    private float epsX=0, epsY=0;
    private float liveTime = 0f;

    private Coroutine coroutineOffHpBar;
    private Base mainBase;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HpBarScale = HpBar.transform.localScale.x;
        HpBar.transform.parent.gameObject.SetActive(false);

        mainBase = GameObject.Find("Base").GetComponent<Base>();


        line = GameObject.Find("MainPath").GetComponent<LineRenderer>();
        epsX = Random.Range(-0.2f, 0.2f);
        epsY = Random.Range(-0.2f, 0.2f);
        transform.position = line.GetPosition(posInd) + new Vector3(epsX, epsY, -2);
        posInd++;

    }

    // Update is called once per frame
    void Update()
    {
        //-------move---------
        Vector3 pos = line.GetPosition(posInd) + new Vector3(epsX, epsY, 0);
        float distance = Mathf.Pow( Mathf.Pow(pos.x - transform.position.x, 2) + Mathf.Pow(pos.y - transform.position.y, 2), 0.5f);
        while (distance < 0.01f)
        {
            posInd++;
            if (posInd == line.positionCount)
                Destroy(gameObject);
            pos = line.GetPosition(posInd) + new Vector3(epsX, epsY, 0);
            distance = Mathf.Pow(Mathf.Pow((pos.x - transform.position.x), 2) + Mathf.Pow((pos.y - transform.position.y), 2), 1 / 2);
        }

        float moveX = (pos.x - transform.position.x) / distance;
        float moveY = (pos.y - transform.position.y) / distance;        

        transform.Translate(new Vector3(moveX, moveY, 0) * speed * Time.deltaTime);

        liveTime += Time.deltaTime;

        //------------hp----------
        if (deltaHpBar > timeHpBar && coroutineOffHpBar==null)
        {
            coroutineOffHpBar = StartCoroutine(offHpBar(0.5f));
        }
        if (HpBar.activeSelf)
        {
            deltaHpBar += Time.deltaTime;
        }
        else
        {
            deltaHpBar = 0;
        }

    }

    public float getLiveTime()
    {
        return liveTime;
    }

    public float getHealth()
    {
        return health;
    }

    public void minusHealth(float damage)
    {
        health -= damage;
        if (health < 0)
        {
            mainBase.addMoney(award);
            Destroy(gameObject);
            return;
        }

        if (coroutineOffHpBar != null)
        {
            StopCoroutine(coroutineOffHpBar);
            coroutineOffHpBar = null;
        }
        setOpacity(1f);
        HpBar.transform.parent.gameObject.SetActive(true);

        UnityEngine.Color color = HpBar.GetComponent<SpriteRenderer>().color;
        UnityEngine.Color colorTargetv = new UnityEngine.Color(0.7f, 0, 0);
        color.r += (colorTargetv.r - color.r) * damage / (health+damage);
        color.g += (colorTargetv.b - color.g) * damage / (health+damage);
        color.b += (colorTargetv.b - color.b) * damage / (health+damage);
        HpBar.GetComponent<SpriteRenderer>().color = color;

        HpBar.transform.localScale = new Vector3(health / maxHealth * HpBarScale, HpBar.transform.localScale.y, HpBar.transform.localScale.z);
        HpBar.transform.SetLocalPositionAndRotation(new Vector3(-(1 - health / maxHealth) * HpBarScale / 2, HpBar.transform.localPosition.y, HpBar.transform.localPosition.z),
                                                    Quaternion.identity);

        deltaHpBar = 0f;
    }

    IEnumerator offHpBar(float t)
    {
        float delta = 0f;
        while (delta < t)
        {
            setOpacity((t - delta) / t);
            delta += Time.deltaTime;
            yield return null;
        }
        HpBar.transform.parent.gameObject.SetActive(false);
        setOpacity(1f);
    } 

    void setOpacity(float opacity)
    {
        UnityEngine.Color color = HpBar.GetComponent<SpriteRenderer>().color; color.a = opacity;
        HpBar.GetComponent<SpriteRenderer>().color = color;
        color = HpBar.transform.parent.gameObject.GetComponent<SpriteRenderer>().color; color.a = opacity;
        HpBar.transform.parent.gameObject.GetComponent<SpriteRenderer>().color = color;
    }
}
