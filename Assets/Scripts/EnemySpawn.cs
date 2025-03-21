using System.Collections;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{

    public GameObject defaultEnemy;
    public float delta = 0.1f;
    public float deltaWave = 30f;
    public int enemyCount = 20;

    private LineRenderer line;
    private float deltaCount = 30f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        line = GameObject.Find("MainPath").GetComponent<LineRenderer>();
        transform.position = line.GetPosition(0) + new Vector3(0, 0, -5);
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
            deltaCount = 0f;
            StartCoroutine(Wave());
        }
    }

    IEnumerator Wave()
    {
        int enemies = 0;
        float timeCount = 0f;
        while (enemies < enemyCount)
        {
            if (timeCount > delta)
            {
                timeCount = 0;
                Instantiate(defaultEnemy);
                enemies++;
            }
            else
            {
                timeCount += Time.deltaTime;
            }
            yield return null;
        }
    }
}
