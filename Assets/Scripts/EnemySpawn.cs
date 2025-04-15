using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{

    public GameObject[] enemiesPrefab;
    private int waveMax = 50;
    private int waveNum = 0;

    private float deltaWave = 15f;
    private int enemyCount = 8;

    private LineRenderer line;
    private Base mainBase;
    private float deltaCount;

    private float wavePower = 0.80f;
    private int waveAward = 25;

    private Dictionary<int, bool[]> waves = new Dictionary<int, bool[]>();
    private ArrayList enemies = new ArrayList();

    private Dictionary<int, float[]> MLData = new Dictionary<int, float[]>(); // number of wave : [waveLiveTime, waveType]

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
        foreach (int wn in MLData.Keys.ToList())
        {
            MLData[wn][0] += Time.deltaTime;
        }


        mainBase.updateWaveUnderText("Next wave in " + (deltaWave - deltaCount).ToString("F1") + "s");
        foreach (int wn in waves.Keys.ToList()) {
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
            if (b) // окончание волны
            {
                GameObject.Find("MLPlayer").GetComponent<MLPlayer>().setMLData(MLData[wn][0], (int)MLData[wn][1]);
                GameObject.Find("MLPlayer").GetComponent<MLPlayer>().endWave(waves[wn][1]);
                GameObject.Find("MLDDA").GetComponent<MLDDA>().setMLData(MLData[wn][0], (int)MLData[wn][1]);
                GameObject.Find("MLDDA").GetComponent<MLDDA>().endWave(waves[wn][1]);
                if (waves[wn][1])
                {
                    mainBase.addMoney(waveAward);
                }
                waves.Remove(wn);
                MLData.Remove(wn);

            }
        }
        if (waveNum >= waveMax && waves.Count == 0)
        {
            mainBase.endLevel(true);
        }
        else if ((deltaCount < deltaWave) && (waves.Count != 0 || waveNum==0))
        {
            deltaCount += Time.deltaTime;
        }
        else
        {
            waveNum++; 
            waves.Add(waveNum, new[] { true, true });
            MLData.Add(waveNum, new[] { 0f, 0f });
            StartCoroutine(Wave());
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
        if (waveNum % 5 == 0 && waveNum!=0)
        {
            enemyCount++;
            waveAward += 5;
        }

        mainBase.updateWaveText("Wave " + waveNum + " / " + waveMax);

        int num = Random.Range(0, enemiesPrefab.Length);
        MLData[waveNum][1] = num;
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
        wavePower += 0.1f;
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

    public ArrayList getEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if ((GameObject)enemies[i] == null)
            {
                enemies.Remove(i);
                i--;
            }
        }


        return enemies;
    }

    public void setDelta(int enemyCountDelta, int waveAwardDelta, float wavePowerDelta)
    {
        enemyCount += enemyCountDelta;
        waveAward += waveAwardDelta;
        wavePower += wavePowerDelta;
    }

    public int getWaveNum()
    {
        return waveNum;
    }
    public int getEnemyCount()
    {
        return enemyCount;
    }
    public int getWaveAward()
    {
        return waveAward;
    }
    public float getWavePower()
    {
        return wavePower;
    }
}
