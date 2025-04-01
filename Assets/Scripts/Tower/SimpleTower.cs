using System.Collections.Generic;
using UnityEngine;

public class SimpleTower : Tower
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        cost = 30;
        rotateSpeed = 0.8f;
        radius = 3f;
        damage = 49;
        attackSpeed = 2.5f;
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
        cost += cost / 2;
    }

    public override void upgrade()
    {
        if (level < maxLevel)
        {
            spentMoney += cost;
            cost += cost / 2;
            rotateSpeed += rotateSpeed / 10;
            radius += radius / 15;
            damage += damage / 10;
            attackSpeed += attackSpeed / 10;
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
            { "Rotate speed", rotateSpeed.ToString("F1") }
        };

        return dict;
    }

    public override string getTowerName()
    {
        return "Simple Tower";
    }

}
