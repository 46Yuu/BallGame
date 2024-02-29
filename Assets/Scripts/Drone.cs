using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    public float speed;
    List<Vector3> path = new List<Vector3>();
    int currentPoint = 0;
    int nextPoint = 1;
    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("DroneRoute"))
        {
            path.Add(go.transform.position);
        }
        transform.position = path[0];
    }

    // Update is called once per frame
    void Update()
    {
        float timeSpeed = speed * Time.deltaTime;
        if (transform.position == path[nextPoint])
        {
            currentPoint = nextPoint;
            nextPoint++;
        }
        if (nextPoint >= path.Count)
        {
            nextPoint = 0;
        }
        transform.position = Vector3.MoveTowards(transform.position, path[nextPoint], timeSpeed);
    }
}
