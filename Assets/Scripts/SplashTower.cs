using System.Collections.Generic;
using UnityEngine;

public class SplashTower : Tower
{

    private float splashRadius;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        cost = 50;
        rotateSpeed = 0.6f;
        radius = 2.5f;
        splashRadius = 0.5f;
        damage = 70;
        attackSpeed = 1;
        maxLevel = 10;

        buy();
        base.Awake();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void buy()
    {
        cost += cost / 5;
    }

    public override void upgrade()
    {
        if (level < maxLevel)
        {
            cost += cost / 5;
            rotateSpeed += rotateSpeed / 10;
            radius += radius / 10;
            damage += damage / 10;
            attackSpeed += attackSpeed / 10;
            splashRadius += splashRadius / 5;
            level++;
        }
    }

    public override Dictionary<string, float> getStats()
    {
        Dictionary<string, float> dict = new Dictionary<string, float>
        {
            { "Damage", damage },
            { "Radius", radius },
            { "AttackSpeed", attackSpeed },
            { "RotateSpeed", rotateSpeed },
            { "SplashRadius", splashRadius },
        };

        return dict;
    }

    public override string getTowerName()
    {
        return "Splash Tower";
    }

    protected override void attack()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            if (checkSplashRadius(enemy.transform.position))
            {
                enemy.GetComponent<Enemy>().minusHealth(damage);
            }
        }
        fire.SetActive(true);
        Invoke("offFire", 0.1f);
    }

    bool checkSplashRadius(Vector3 v)
    {
        return (Mathf.Pow((v.x - target.transform.position.x), 2) + Mathf.Pow((v.y - target.transform.position.y), 2)) < splashRadius * splashRadius;
    }

}
