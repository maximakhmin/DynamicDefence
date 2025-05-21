using UnityEngine;

public class FastEnemy : Enemy
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        baseSpeed = 2.75f;
        maxHealth = 33;
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
