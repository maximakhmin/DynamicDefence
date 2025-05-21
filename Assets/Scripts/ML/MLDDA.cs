using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.Collections.Generic;

public class MLDDA : Agent
{

    private List<Vector3> lastHitPositions = new List<Vector3>();
    private LineRenderer line;
    private EnemySpawn enemySpawn;

    private bool isChange = false;
    private float pathDistance;

    private static int numObservations = 5;

    //ml parametres
    private List<int>   waveEnemyTypeList       = new List<int>();
    private List<float> waveLiveTimeList        = new List<float>();
    private List<float> lastDistanceList        = new List<float>();
    private List<float> lastHitDistanceList     = new List<float>();
    private List<float> meanLastHitDistanceList = new List<float>();


    void Start()
    {
        line = GameObject.Find("MainPath").GetComponent<LineRenderer>();
        enemySpawn = GameObject.Find("EnemySpawn").GetComponent<EnemySpawn>();
        pathDistance = calculatePathDistance(line.GetPosition(line.positionCount-1));

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(isChange);
        sensor.AddObservation(pathDistance);

        //sensor.AddObservation(enemySpawn.getWaveNum());
        sensor.AddObservation(enemySpawn.getEnemyCount());
        sensor.AddObservation(enemySpawn.getWaveAward());
        sensor.AddObservation(enemySpawn.getWavePower());

        for (int i = 0; i < numObservations; i++)
        {
            if (waveLiveTimeList.Count > i)
            {
                //sensor.AddObservation(waveLiveTimeList[i]);
                sensor.AddObservation(waveEnemyTypeList[i]);
                sensor.AddObservation(lastDistanceList[i]);
                sensor.AddObservation(lastHitDistanceList[i]);
                //sensor.AddObservation(meanLastHitDistanceList[i]);
            }
            else
            {
                for (int j = 0; j < 3; j++)
                {
                    sensor.AddObservation(-1);
                }
            }
        }

        
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        if (isChange)
        {
            int enemyCountDelta = actions.DiscreteActions[0] - 1; //3
            int waveAwardDelta = actions.DiscreteActions[1] - 5; //11
            float wavePowerDelta = actions.ContinuousActions[0] * 0.2f;
            //enemySpawn.setDelta(enemyCountDelta, waveAwardDelta, wavePowerDelta);
            Debug.Log(enemyCountDelta + " " + waveAwardDelta + " " + wavePowerDelta +" \t\t\tReward: " + GetCumulativeReward());
            isChange = false;
        }
    }

    public void addLastHitPosition(Vector3 pos)
    {
        lastHitPositions.Add(pos);
    }

    public void setMLData(float lt, int et)
    {
        addToList(waveLiveTimeList, lt);
        addToList(waveEnemyTypeList, et);
    }

    public void endWave(bool b)
    {
        addToList(lastDistanceList, towersLastDistance());
        if (lastHitPositions.Count == 0)
        {
            lastHitPositions.Add(line.GetPosition(line.positionCount - 1));
        }
        if (!b) addToList(lastHitDistanceList, pathDistance);
        else addToList(lastHitDistanceList, calculatePathDistance(lastHitPositions[lastHitPositions.Count - 1]));

        float temp = 0;
        foreach (Vector3 pos in lastHitPositions)
        {
            temp += calculatePathDistance(pos);
        }
        addToList(meanLastHitDistanceList, temp / lastHitPositions.Count);

        float ld = lastDistanceList[lastDistanceList.Count - 1];
        float lhd = lastHitDistanceList[lastHitDistanceList.Count - 1];

        if (!b)
        {
            SetReward(-5f);
            Debug.Log("-0.5");
        }

        SetReward((-Mathf.Abs(ld - lhd) / pathDistance + 0.3f) * 10);
        Debug.Log(-Mathf.Abs(ld - lhd) / pathDistance + 0.3f);

        lastHitPositions = new List<Vector3>();
        isChange = true;
    }

    float towersLastDistance()
    {
        float step = 0.1f;
        float ans = 0;
        float levelSum = 0;

        foreach (GameObject gm in GameObject.FindGameObjectsWithTag("Tower"))
        {
            Tower t = gm.GetComponent<Tower>();
            if (t.getTowerNum() == 3) continue;
            Vector3 point = line.GetPosition(0);
            int iter = 1;
            float path = 0;
            float longestTowerPath = 0;
            while (path < 1000f)
            {
                if (t.isInRadius(point))
                {
                    longestTowerPath = path;
                }

                float dist = Mathf.Pow((Mathf.Pow(point.x - line.GetPosition(iter).x, 2) + Mathf.Pow(point.y - line.GetPosition(iter).y, 2)), 0.5f);
                if (dist <= step/2)
                {
                    iter++;
                    if (iter == line.positionCount) break;
                    dist = Mathf.Pow((Mathf.Pow(line.GetPosition(iter).x - point.x, 2) + Mathf.Pow(line.GetPosition(iter).y - point.y, 2)), 0.5f);

                }
                float moveX = (line.GetPosition(iter).x - point.x) / dist;
                float moveY = (line.GetPosition(iter).y - point.y) / dist;
                point += new Vector3(moveX, moveY, 0) * step;
                path += step;
            }

            ans += longestTowerPath * t.getLevel();
            levelSum += t.getLevel();
        }
        if (levelSum == 0) return 0;
        else return ans / levelSum;
    }

    float calculatePathDistance(Vector3 pos)
    {
        float step = 0.1f;
         
        Vector3 point = line.GetPosition(0);
        int iter = 1;
        float path = 0;
        while (path < 1000f)
        {
            if (Mathf.Pow((Mathf.Pow(point.x - pos.x, 2) + Mathf.Pow(point.y - pos.y, 2)), 0.5f) < 0.2f + step)
            {
                return path;
            }


            float dist = Mathf.Pow((Mathf.Pow(point.x - line.GetPosition(iter).x, 2) + Mathf.Pow(point.y - line.GetPosition(iter).y, 2)), 0.5f);
            if (dist <= step/2)
            {
                iter++;
                if (iter == line.positionCount) break;
                dist = Mathf.Pow((Mathf.Pow(line.GetPosition(iter).x - point.x, 2) + Mathf.Pow(line.GetPosition(iter).y - point.y, 2)), 0.5f);

            }
            float moveX = (line.GetPosition(iter).x - point.x) / dist;
            float moveY = (line.GetPosition(iter).y - point.y) / dist;
            point += new Vector3(moveX, moveY, 0) * step;
            path += step;
        }

        return path;
    }

    public void endEpisodeVoid()
    {
        //SetReward(GetCumulativeReward() / enemySpawn.getWaveNum());

        EndEpisode();
    }

    void addToList<T>(List<T> list, T val)
    {
        if (list.Count == numObservations)
        {
            list.RemoveAt(0);
            list.Add(val);
        }
        else
        {
            list.Add(val);
        }
    }
}
