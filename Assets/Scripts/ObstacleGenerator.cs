using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    public List<GameObject> listObstacles = new List<GameObject>();
    public List<GameObject> createdObstacles = new List<GameObject>();
    private MeshGenerator meshGenerator;

    // Start is called before the first frame update
    void Start()
    {
        meshGenerator = FindAnyObjectByType<MeshGenerator>();
        Random.InitState(meshGenerator.initialSeed);
        generateObstacles();
    }

    void generateObstacles()
    {
        int numObstacles = Random.Range(5, 15);
        for(int i = 0; i < numObstacles; i++)
        {
            int scaleObstacle = Random.Range(2, 7);
            int indexObstacle = Random.Range(0, listObstacles.Count);
            GameObject obstacle = Instantiate(listObstacles[indexObstacle], new Vector3(Random.Range(-(meshGenerator.xSize), meshGenerator.xSize)/2, Random.Range(0,10), Random.Range(-(meshGenerator.zSize), meshGenerator.zSize)/2), Quaternion.identity);
            obstacle.transform.localScale = new Vector3(scaleObstacle, scaleObstacle, scaleObstacle);
            createdObstacles.Add(obstacle);
        }
    }

}
