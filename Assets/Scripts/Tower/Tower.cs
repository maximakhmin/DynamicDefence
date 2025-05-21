using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Tower : MonoBehaviour
{

    public GameObject tower;
    public GameObject fire;

    protected float rotateSpeed;
    protected float radius;
    protected float damage;
    protected float attackSpeed;
    private float attackDelta = 0f;
    protected int cost;
    protected int spentMoney = 0;
    private int targetMode = 0;
    protected int level = 1;
    protected int maxLevel = 10;

    protected GameObject target;


    protected virtual void Awake()
    {

    }

    protected virtual void Update()
    {
        target = findEmeny();

        if (target)
        {
            if (rotateToTarget() && attackDelta > 1/attackSpeed)
            {
                attackDelta = 0;
                attack();
            }
        }
        attackDelta += Time.deltaTime;
    }

    GameObject findEmeny()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject e = null;
        float temp;
        switch (targetMode) {
            default:
                return e;
            case 0: // first
                temp = 0;
                foreach (GameObject enemy in enemies)
                {
                    if ((enemy.GetComponent<Enemy>().getLiveTime() >= temp) &&
                          isInRadius(enemy.transform.position))
                    {
                        e = enemy;
                        temp = enemy.GetComponent<Enemy>().getLiveTime();
                    }
                }
                return e;
            case 1: // last
                if (enemies.Length == 0) return null;
                temp = enemies[0].GetComponent<Enemy>().getLiveTime();
                foreach (GameObject enemy in enemies)
                {
                    if ((enemy.GetComponent<Enemy>().getLiveTime() <= temp) &&
                          isInRadius(enemy.transform.position))
                    {
                        e = enemy;
                        temp = enemy.GetComponent<Enemy>().getLiveTime();
                    }
                }
                return e;
            case 2: // highest health
                temp = 0;
                foreach (GameObject enemy in enemies)
                {
                    if ((enemy.GetComponent<Enemy>().getHealth() >= temp) &&
                          isInRadius(enemy.transform.position))
                    {
                        e = enemy;
                        temp = enemy.GetComponent<Enemy>().getHealth();
                    }
                }
                return e;
            case 3: // lowest health
                if (enemies.Length == 0) return null;
                temp = enemies[0].GetComponent<Enemy>().getHealth();
                foreach (GameObject enemy in enemies)
                {
                    if ((enemy.GetComponent<Enemy>().getHealth() <= temp) &&
                          isInRadius(enemy.transform.position))
                    {
                        e = enemy;
                        temp = enemy.GetComponent<Enemy>().getHealth();
                    }
                }
                return e;
            case 4: // random
                if (target != null && isInRadius(target.transform.position))
                    return target;
                else
                {
                    GameObject[] enemiesInRadius = new GameObject[enemies.Length];
                    int it = 0;
                    for (int i = 0; i < enemies.Length; i++)
                    {
                        if (isInRadius(enemies[i].transform.position))
                        {
                            enemiesInRadius[it] = enemies[i];
                            it++;
                        }
                    }
                    return enemiesInRadius[Random.Range(0, it)];
                }

        }
    }
    public bool isInRadius(Vector3 v)
    {
        return (Mathf.Pow((v.x - transform.position.x), 2) + Mathf.Pow((v.y - transform.position.y), 2)) <= radius * radius;
    }

    bool rotateToTarget()
    {
        float angle = targetAngle();
        if (Mathf.Abs(angle - tower.transform.rotation.eulerAngles.z) < rotateSpeed * 180 * Time.deltaTime)
        {
            tower.transform.rotation = Quaternion.Euler(0, 0, angle);
            return true;
        }
        else
        {
            float rotateTower = tower.transform.rotation.eulerAngles.z;
            int rotate;
            if (rotateTower < 180)
            {
                rotate = ((angle > rotateTower) && (angle < (rotateTower + 180) % 360)) ? 1 : -1;
            }
            else
            {
                rotate = ((angle > rotateTower) || (angle < (rotateTower + 180) % 360)) ? 1 : -1;
            }
            tower.transform.rotation = Quaternion.Euler(0, 0, (rotateTower + rotate * rotateSpeed * 180 * Time.deltaTime) % 360);
            return false;
        }
    }

    float targetAngle()
    {
        float distance = Mathf.Pow((Mathf.Pow((target.transform.position.x - transform.position.x), 2) +
                                     Mathf.Pow((target.transform.position.y - transform.position.y), 2)), 0.5f);
        float angle = -Mathf.Asin((target.transform.position.x - transform.position.x) / distance) * 180 / Mathf.PI;

        if (target.transform.position.y - transform.position.y < 0)
        {
            angle = 180 - angle;
        }
        if (angle < 0)
        {
            angle += 360;
        }

        return angle;

    }

    protected virtual void attack()
    {
        target.GetComponent<Enemy>().minusHealth(damage);
        playFire();
    }
    protected void playFire()
    {
        fire.GetComponent<Animator>().Play("Explosion2", 0, 0f);
    }

    public void setTargetMode(int t)
    {
        targetMode = t;
    }
    public int getTargetMode()
    {
        return targetMode;
    }
    public int getSellCost()
    {
        return spentMoney / 2;
    }
    public float getRadius()
    {
        return radius;
    }
    public int getCost()
    {
        return cost;
    }
    public string getLevelStr()
    {
        return level + "/" + maxLevel + " lvl";
    }
    public int getLevel()
    {
        return level;
    }
    public bool isMax()
    {
        return level == maxLevel;
    }

    public abstract void upgrade();
    public abstract void buy();
    public abstract Dictionary<string, string> getStats();
    public abstract string getTowerName();
    public abstract int getTowerNum();
}


