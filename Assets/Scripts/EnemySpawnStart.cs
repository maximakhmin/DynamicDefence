using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawnStart : MonoBehaviour
{

    public GameObject[] enemiesPrefab;

    private float deltaWave = 10f;
    private int enemyCount = 10;

    private float deltaCount;

    private float wavePower = 1.1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        deltaCount = 5f;
    }

    // Update is called once per frame
    void Update()
    { 
        if (deltaCount < deltaWave)
        {
            deltaCount += Time.deltaTime;
        }
        else
        {
            StartCoroutine(Wave());
            deltaCount = 0f;

        }
    }

    IEnumerator Wave()
    {
        int num = Random.Range(0, enemiesPrefab.Length);
        float delta = 0f;

        int count = 0;
        float timeCount = 0f;
        while (count < enemyCount)
        {
            if (timeCount > delta)
            {
                timeCount = 0;
                GameObject gm = Instantiate(enemiesPrefab[num]);
                gm.GetComponent<Enemy>().setEnemy(wavePower, 0);
                delta = -0.1f * gm.GetComponent<Enemy>().getSpeed() + 0.4f;
                count++;
            }
            else
            {
                timeCount += Time.deltaTime;
            }
            yield return null;
        }
    }


}
