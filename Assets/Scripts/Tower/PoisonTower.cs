using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PoisonTower : Tower
{
    private float poisonDuration; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        cost = 80;
        rotateSpeed = 1.5f;
        radius = 1.5f;
        damage = 30; // per second
        attackSpeed = 1.5f;
        poisonDuration = 3f;
        maxLevel = 10;

        setTargetMode(2);
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
        cost += (int)(cost * 0.35f);
    }

    public override void upgrade()
    {
        if (level < maxLevel)
        {
            spentMoney += cost;
            cost += (int)(cost * 0.4f);
            rotateSpeed += rotateSpeed * 0.1f;
            radius += radius * 0.05f;
            damage += damage * 0.15f;
            attackSpeed += attackSpeed * 0.1f;
            poisonDuration += 0.225f;
            level++;
        }
    }

    public override Dictionary<string, string> getStats()
    {
        Dictionary<string, string> dict = new Dictionary<string, string>
        {
            { "Damage per second", damage.ToString("F1") },
            { "Radius", radius.ToString("F1") },
            { "Attack speed", attackSpeed.ToString("F1") },
            { "Rotate speed", rotateSpeed.ToString("F1") },
            { "Poison duration", poisonDuration.ToString("F1") },
        };

        return dict;
    }

    public override string getTowerName()
    {
        return "Poison Tower";
    }

    public override int getTowerNum()
    {
        return 4;
    }

    protected override void attack()
    {
        playFire();
        target.GetComponent<Enemy>().poison(damage, poisonDuration);
    }

}
