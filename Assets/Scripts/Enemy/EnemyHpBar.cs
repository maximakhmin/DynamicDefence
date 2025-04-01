using System.Collections;
using UnityEngine;

public class EnemyHpBar : MonoBehaviour
{

    public GameObject HpBar;
    public GameObject freezeMark;
    public GameObject poisonMark;
    private float HpBarScale;

    private float timeHpBar = 2f;
    private float deltaHpBar = 0f;

    private Coroutine coroutineOffHpBar;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HpBarScale = HpBar.transform.localScale.x;
        HpBar.transform.parent.gameObject.SetActive(false);
        offFreezeMark(); 
        offPoisonMark();
    }

    // Update is called once per frame
    void Update()
    {
        if (deltaHpBar > timeHpBar && coroutineOffHpBar == null)
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

    public void minusHealth(float damage, float health, float maxHealth)
    {
        if (coroutineOffHpBar != null)
        {
            StopCoroutine(coroutineOffHpBar);
            coroutineOffHpBar = null;
        }
        setOpacity(1f);
        HpBar.transform.parent.gameObject.SetActive(true);

        UnityEngine.Color color = HpBar.GetComponent<SpriteRenderer>().color;
        UnityEngine.Color colorTargetv = new UnityEngine.Color(0.7f, 0, 0);
        color.r += (colorTargetv.r - color.r) * damage / (health + damage);
        color.g += (colorTargetv.b - color.g) * damage / (health + damage);
        color.b += (colorTargetv.b - color.b) * damage / (health + damage);
        HpBar.GetComponent<SpriteRenderer>().color = color;

        HpBar.transform.localScale = new Vector3(health / maxHealth * HpBarScale, HpBar.transform.localScale.y, HpBar.transform.localScale.z);
        HpBar.transform.SetLocalPositionAndRotation(new Vector3(-(1 - health / maxHealth) * HpBarScale / 2, HpBar.transform.localPosition.y, HpBar.transform.localPosition.z),
                                                    Quaternion.identity);

        deltaHpBar = 0f;
    }
    public void onFreezeMark()
    {
        if (coroutineOffHpBar != null)
        {
            StopCoroutine(coroutineOffHpBar);
            coroutineOffHpBar = null;
        }
        setOpacity(1f);
        HpBar.transform.parent.gameObject.SetActive(true);
        deltaHpBar = 0f;
        freezeMark.SetActive(true);
    }
    public void offFreezeMark()
    {
        freezeMark.SetActive(false);
    }

    public void onPoisonMark()
    {
        if (coroutineOffHpBar != null)
        {
            StopCoroutine(coroutineOffHpBar);
            coroutineOffHpBar = null;
        }
        setOpacity(1f);
        HpBar.transform.parent.gameObject.SetActive(true);
        deltaHpBar = 0f;
        poisonMark.SetActive(true);
    }
    public void offPoisonMark()
    {
        poisonMark.SetActive(false);
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
