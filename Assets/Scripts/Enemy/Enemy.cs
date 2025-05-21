using System.Collections;
using System.Drawing;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public abstract class Enemy : MonoBehaviour
{
    public GameObject sprite;

    protected float baseSpeed ;
    private float speed;
    protected int power ;
    protected int award ;

    private float health;
    protected float maxHealth;

    private LineRenderer line;
    private int posInd = 0;
    private float epsX=0, epsY=0;
    private float liveTime = 0f;

    private Base mainBase;
    private Coroutine freezeCoroutine;
    private Coroutine poisonCoroutine;

    private Vector3 lastHitPosition;

    private int waveNum;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Awake()
    {
        mainBase = GameObject.Find("Base").GetComponent<Base>();
        speed = baseSpeed;
        health = maxHealth;

        line = GameObject.Find("MainPath").GetComponent<LineRenderer>();
        epsX = Random.Range(-0.2f, 0.2f);
        epsY = Random.Range(-0.2f, 0.2f);
        transform.position = line.GetPosition(posInd) + new Vector3(epsX, epsY, -2);
        lastHitPosition = transform.position;
        posInd++;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Vector3 pos = line.GetPosition(posInd) + new Vector3(epsX, epsY, 0);
        float distance = Mathf.Pow( Mathf.Pow(pos.x - transform.position.x, 2) + Mathf.Pow(pos.y - transform.position.y, 2), 0.5f);
        while (distance < Time.deltaTime * baseSpeed)
        {
            posInd++;
            if (posInd == line.positionCount)
                Destroy(gameObject);
            pos = line.GetPosition(posInd) + new Vector3(epsX, epsY, 0);
            distance = Mathf.Pow(Mathf.Pow((pos.x - transform.position.x), 2) + Mathf.Pow((pos.y - transform.position.y), 2), 0.5f);
        }
        float moveX = (pos.x - transform.position.x) / distance;
        float moveY = (pos.y - transform.position.y) / distance;
        float angle = -Mathf.Asin((pos.x - transform.position.x) / distance) * 180 / Mathf.PI;
        if (moveY < 0)
        {
            angle = 180 - angle;
        }
        sprite.transform.rotation = Quaternion.Euler(0, 0, angle);

        transform.Translate(new Vector3(moveX, moveY, 0) * speed * Time.deltaTime);

        liveTime += Time.deltaTime;
    }

    public float getLiveTime()
    {
        return liveTime;
    }

    public float getHealth()
    {
        return health;
    }

    public int getPower()
    {
        return power;
    }

    public void setEnemy(float p, int wn)
    {
        waveNum = wn;
        maxHealth *= p;
        health = maxHealth;
    }
    public float getSpeed()
    {
        return baseSpeed;
    }
    public int getWaveNum()
    {
        return waveNum;
    }

    public void minusHealth(float damage)
    {
        health -= damage;
        lastHitPosition = transform.position;
        if (health < 0)
        {
            GameObject gm = GameObject.Find("MLDDA");
            if (gm) gm.GetComponent<MLDDA>().addLastHitPosition(lastHitPosition);
            gm = GameObject.Find("Music");
            if (gm) gm.GetComponent<SoundController>().playEmenyDeathSound();

            mainBase.addMoney(award);
            Destroy(gameObject);
            return;
        }

        gameObject.GetComponent<EnemyHpBar>().minusHealth(damage, health, maxHealth);
    }

    public void freeze(float freezePower, float freezeTime)
    {
        speed = baseSpeed * (1 - freezePower);
        if (freezeCoroutine!=null)
        {
            StopCoroutine(freezeCoroutine);
        }
        freezeCoroutine = StartCoroutine(offFreeze(freezeTime));
        gameObject.GetComponent<EnemyHpBar>().onFreezeMark();
    }

    IEnumerator offFreeze(float freezeTime)
    {
        float delta = 0;
        while (delta < freezeTime)
        {
            delta += Time.deltaTime;
            yield return null;
        }
        gameObject.GetComponent<EnemyHpBar>().offFreezeMark();
        speed = baseSpeed;

    }

    public void poison(float damage, float durartion)
    {
        if (poisonCoroutine != null)
        {
            StopCoroutine(poisonCoroutine);
        }
        poisonCoroutine = StartCoroutine(offPoison(damage, durartion));
        gameObject.GetComponent<EnemyHpBar>().onPoisonMark();
    }

    IEnumerator offPoison(float damage, float duration)
    {
        float delta = 0;
        lastHitPosition = transform.position;
        while (delta < duration)
        {
            delta += Time.deltaTime;
            minusHealth(Time.deltaTime * damage);
            yield return null;
        }
        gameObject.GetComponent<EnemyHpBar>().offPoisonMark();
    }

}
