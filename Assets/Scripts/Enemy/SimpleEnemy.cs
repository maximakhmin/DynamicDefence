using UnityEngine;

public class SimpleEnemy : Enemy
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        baseSpeed = 1.5f;
        maxHealth = 60;
        power = 1;
        award = 1;

        base.Awake();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
