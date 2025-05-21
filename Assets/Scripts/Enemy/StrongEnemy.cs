using UnityEngine;

public class StrongEnemy : Enemy
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        baseSpeed = 1;
        maxHealth = 100;
        power = 2;
        award = 1;

        base.Awake();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
