using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FreezeTower : Tower
{
    private float freezePower;
    private float freezeTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        cost = 70;
        radius = 2.5f;
        maxLevel = 5;
        freezePower = 0.3f;
        freezeTime = 1f;
        base.Awake();
    }

    // Update is called once per frame
    protected override void Update()
    {
        tower.transform.Rotate(new Vector3(0, 0, -1) * 300 * Time.deltaTime);
        attack();
    }

    public override void buy()
    {
        spentMoney += cost;
        cost += cost/2;
    }

    public override void upgrade()
    {
        if (level < maxLevel)
        {
            spentMoney += cost;
            cost += cost/2;
            radius += 0.375f;
            freezePower += 0.05f;
            freezeTime += 0.25f;
            level++;
        }
    }

    public override Dictionary<string, string> getStats()
    {
        Dictionary<string, string> dict = new Dictionary<string, string>
        {
            { "Radius", radius.ToString("F1") },
            { "Freeze power", (freezePower*100).ToString("F0")+"%" },
            { "Freeze time", freezeTime.ToString("F2") },
        };

        return dict;
    }

    public override string getTowerName()
    {
        return "Freeze Tower";
    }

    protected override void attack()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            if (isInRadius(enemy.transform.position))
            {
                enemy.GetComponent<Enemy>().freeze(freezePower, freezeTime);
            }
        }
    }

}
