using System.Collections.Generic;
using UnityEngine;

public class SplashTower : Tower
{

    private float splashRadius;
    public GameObject explosion;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        cost = 50;
        rotateSpeed = 0.6f;
        radius = 2f;
        splashRadius = 0.5f;
        damage = 60;
        attackSpeed = 0.8f;
        maxLevel = 10;

        base.Awake();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void buy()
    {
        spentMoney += cost;
        cost += (int)(cost * 0.4f);
    }

    public override void upgrade()
    {
        if (level < maxLevel)
        {
            spentMoney += cost;
            cost += (int)(cost * 0.4f);
            rotateSpeed += rotateSpeed / 10;
            radius += radius * 0.07f;
            damage += damage / 10;
            attackSpeed += attackSpeed / 10;
            splashRadius += splashRadius / 15;
            level++;
        }
    }

    public override Dictionary<string, string> getStats()
    {
        Dictionary<string, string> dict = new Dictionary<string, string>
        {
            { "Damage", damage.ToString("F1") },
            { "Radius", radius.ToString("F1") },
            { "Attack speed", attackSpeed.ToString("F1") },
            { "Rotate speed", rotateSpeed.ToString("F1") },
            { "Splash radius", splashRadius.ToString("F1") },
        };

        return dict;
    }

    public override string getTowerName()
    {
        return "Splash Tower";
    }

    public override int getTowerNum()
    {
        return 2;
    }

    protected override void attack()
    {
        playFire();

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            if (checkSplashRadius(enemy.transform.position))
            {
                enemy.GetComponent<Enemy>().minusHealth(damage);
            }
        }

        GameObject gm = Instantiate(explosion, target.transform.position, Quaternion.identity);
        gm.transform.localScale *= splashRadius;

    }

    bool checkSplashRadius(Vector3 v)
    {
        return (Mathf.Pow((v.x - target.transform.position.x), 2) + Mathf.Pow((v.y - target.transform.position.y), 2)) < splashRadius * splashRadius;
    }

}
