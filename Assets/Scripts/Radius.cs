using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class Radius : MonoBehaviour
{
    public int iteratinons = 100;

    private LineRenderer line;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = iteratinons;
        line.enabled = false;
    }

    public void createRadius(Vector3 pos, float radius)
    {
        line.enabled = true;
        for (int i = 0; i < iteratinons; i++)
        {
            line.SetPosition(i, new Vector3(pos.x + Mathf.Cos(2 * Mathf.PI * i / iteratinons) * radius,
                                            pos.y + Mathf.Sin(2 * Mathf.PI * i / iteratinons) * radius,
                                            pos.z));
        }
    }

    public void offRadius()
    {
        line.enabled = false;
    }


}
