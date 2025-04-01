using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{

    public GameObject[] enemiesPrefab;
    public int waveMax = 100;
    private int waveNum = 0;

    private float deltaWave = 15f;
    private int enemyCount = 10;

    private LineRenderer line;
    private Base mainBase;
    private float deltaCount;

    private float wavePower = 1;
    private int waveAward = 35;

    private Dictionary<int, bool[]> waves = new Dictionary<int, bool[]>();
    private ArrayList enemies = new ArrayList();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        line = GameObject.Find("MainPath").GetComponent<LineRenderer>();
        mainBase = GameObject.Find("Base").GetComponent<Base>();
        mainBase.updateWaveText("Wave " + waveNum + " / " + waveMax);

        transform.position = line.GetPosition(0) + new Vector3(0, 0, -5);

        deltaCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        mainBase.updateWaveUnderText("Next wave in " + (deltaWave - deltaCount).ToString("F1") + "s");
        foreach (int wn in waves.Keys) {
            if (waves[wn][0])
            {
                continue;
            }
            bool b = true;
            for (int i = 0; i < enemies.Count; i++)
            {
                if ( (GameObject)enemies[i]!=null && wn == ((GameObject)enemies[i]).GetComponent<Enemy>().getWaveNum() )
                {
                    b = false;
                    break;
                }
            }
            if (b)
            {
                if (waves[wn][1])
                {
                    mainBase.addMoney(waveAward);
                }
                waves.Remove(wn);
            }
        }

        if ((deltaCount < deltaWave) && (waves.Count != 0 || waveNum==0))
        {
            deltaCount += Time.deltaTime;
        }
        else
        {
            StartCoroutine(Wave());
            waves.Add(waveNum, new[] { true, true });
            deltaCount = 0f;

            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i]==null)
                {
                    enemies.Remove(i);
                    i--;
                }
            }
        }
    }

    IEnumerator Wave()
    {
        waveNum++;
        if (waveNum % 5 == 0)
        {
            enemyCount++;
            waveAward += 3;
        }

        mainBase.updateWaveText("Wave " + waveNum + " / " + waveMax);

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
                gm.GetComponent<Enemy>().setEnemy(wavePower, waveNum);
                enemies.Add(gm);
                delta = -0.1f * gm.GetComponent<Enemy>().getSpeed() + 0.4f;
                count++;
            }
            else
            {
                timeCount += Time.deltaTime;
            }
            yield return null;
        }
        waves[waveNum][0] = false;
        wavePower += 0.10f;
    }

    public void offAward(int WaveNumber)
    {
        foreach (int wn in waves.Keys)
        {
            if (wn == WaveNumber)
            {
                waves[wn][1] = false;
                return;
            }
        }
    }
}
