using System.Collections;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;
using UnityEngine;

public class MLPlayer : Agent
{
    private Base mainBase;
    public SpriteRenderer background;
    private LineRenderer line;

    public GameObject[] platforms;
    private GameObject[] towers;
    public GameObject[] towerPrefabs;
    private int[] towerMoney = new int[] { 30, 50, 70, 80 };
    private int waveEnemyType = -1;
    private float waveLiveTime = 0;

    void Start()
    {
        mainBase = GameObject.Find("Base").GetComponent<Base>();
        line = GameObject.Find("MainPath").GetComponent<LineRenderer>();

        towers = new GameObject[platforms.Length];
        for (int i = 0; i < towers.Length; i++)
        {
            towers[i] = null;
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(mainBase.getMoney());
        sensor.AddObservation(mainBase.getHealth());

        sensor.AddObservation(waveEnemyType);
        sensor.AddObservation(waveLiveTime);

        for (int i = 0; i < line.positionCount; i++)
        {
            sensor.AddObservation(line.GetPosition(i));
        }

        for (int i = 0; i < platforms.Length; i++)
        {
            sensor.AddObservation(platforms[i].transform.position);
            if (towers[i] == null)
            {
                sensor.AddObservation(0);
                sensor.AddObservation(0);
            }
            else
            {
                sensor.AddObservation(towers[i].GetComponent<Tower>().getTowerNum());
                sensor.AddObservation(towers[i].GetComponent<Tower>().getLevel());
            }
        }

        base.CollectObservations(sensor);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int platformNum = actions.DiscreteActions[0];
        int towerType = actions.DiscreteActions[1];
        int actionType = actions.DiscreteActions[2];
        switch (actionType)
        {
            case 0: //nothing
                break;
            case 1: //buy
                if ((towers[platformNum] == null) && (mainBase.getMoney() > towerMoney[towerType]))
                {
                    mainBase.addMoney(-towerMoney[towerType]);
                    towers[platformNum] = Instantiate(towerPrefabs[towerType], platforms[platformNum].transform.position, Quaternion.identity);
                }
                break;
            case 2: //upgrade
                if (towers[platformNum]!=null && (mainBase.getMoney() > towers[platformNum].GetComponent<Tower>().getCost()))
                {
                    mainBase.addMoney(-towers[platformNum].GetComponent<Tower>().getCost());
                    towers[platformNum].GetComponent<Tower>().upgrade();
                }
                break;
            /*case 3: //sell
                if (towers[platformNum]!=null)
                {
                    mainBase.addMoney(towers[platformNum].GetComponent<Tower>().getSellCost());
                    Destroy(towers[platformNum]);
                    towers[platformNum] = null;
                }
                break;*/
        }



        base.OnActionReceived(actions);
    }



    public void endEpisodeVoid(bool b = false)
    {
        if (b)
        {
            AddReward(5f);
        }
        else
        {
            AddReward(-10f);
        }
        EndEpisode();
    }

    public void endWave(bool b)
    {
        if (b)
        {
            AddReward(1f);
            for (int i = 1; i <= 4; i++)
            {
                foreach (GameObject t in towers)
                {
                    if (t!=null && t.GetComponent<Tower>().getTowerNum() == i)
                    {
                        AddReward(0.25f); // награда за разные башни
                        break;
                    }
                }
            }
            //background.color = new Color(0f, 0.3f, 0f, 1f);

        }
        else
        {
            AddReward(-5f);
            //background.color = new Color(0.3f, 0f, 0f, 1f);
        }
    }

    public void setMLData(float dt, int et)
    {
        waveLiveTime = dt;
        waveEnemyType = et;
    }






}
