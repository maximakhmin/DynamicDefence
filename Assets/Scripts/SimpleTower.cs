using System.Collections.Generic;
using UnityEngine;

public class SimpleTower : Tower
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        cost = 20;
        rotateSpeed = 0.7f;
        radius = 3.5f;
        damage = 34;
        attackSpeed = 4;
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
            { "RotateSpeed", rotateSpeed }
        };

        return dict;
    }

    public override string getTowerName()
    {
        return "Simple Tower";
    }

}
