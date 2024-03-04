using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    public List<GameObject> listObstacles = new List<GameObject>();
    private List<Transform> SpawnPointsCannon = new List<Transform>();
    private MeshGenerator meshGenerator;
    public GameObject cannon;
    public int numCannons;

    void Awake()
    {
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("CannonSpawnPoint"))
        {
            SpawnPointsCannon.Add(gameObject.GetComponent<Transform>());
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        meshGenerator = FindAnyObjectByType<MeshGenerator>();
        Random.InitState(meshGenerator.initialSeed);
        GetComponent<Transform>().position = new Vector3(-(meshGenerator.xSize/2),0,-(meshGenerator.zSize/2)+15);
        generateObstacles();
        generateCannon();
    }

    void generateObstacles()
    {
        int numObstacles = Random.Range(2 , meshGenerator.xSize / 10);
        Vector2 sample_zone = new Vector2(meshGenerator.xSize, meshGenerator.zSize-30);
        List<Vector2> tempSamples = GeneratePoint(20, sample_zone, numObstacles);
        if (tempSamples != null)
        {
            foreach(Vector2 sample in tempSamples)
            {
                int scaleRock = Random.Range(3, 5);
                int indexObstacle = Random.Range(0, listObstacles.Count);
                GameObject obstacle = Instantiate(listObstacles[indexObstacle], new Vector3(sample.x, Random.Range(meshGenerator.minTerrainHeight,meshGenerator.maxTerrainHeight), sample.y)+transform.position, Quaternion.identity);
                obstacle.transform.localScale = new Vector3(scaleRock, scaleRock, scaleRock);
            }
        }
    }

    static bool is_valid(List<Vector2> samples, int[,] grid, Vector2 sample, Vector2 sample_zone,float radius, float cell_size)
    {
        if(sample.x < sample_zone.x && sample.x >= 0 && sample.y < sample_zone.y && sample.y >= 0)
        {
            int x = (int)(sample.x / cell_size);
            int y = (int)(sample.y / cell_size);
            int offset_x = Mathf.Max(0, x - 2);
            int out_x = Mathf.Min(x + 2, grid.GetLength(0) - 1);
            int offset_y = Mathf.Max(0, y - 2);
            int out_y = Mathf.Min(y + 2, grid.GetLength(1) - 1);

            for (int i = offset_x; i <= out_x; i++)
            {
                for (int j = offset_y; j <= out_y; j++)
                {
                    int s_index = grid[i, j] - 1;
                    if(s_index != -1)
                    {
                        float _distance = (sample - samples[s_index]).sqrMagnitude;
                        if(_distance < radius*radius) 
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        return false;
    } 
    public static List<Vector2> GeneratePoint(float radius, Vector2 grid_size, int k  = 15)
    {
        float cell_size = radius / Mathf.Sqrt(2);

        int[,] grid = new int[Mathf.CeilToInt(grid_size.x / cell_size), Mathf.CeilToInt(grid_size.y / cell_size)];
        
        List<Vector2> samples = new List<Vector2>();
        List<Vector2> spawn_samples = new List<Vector2>();

        spawn_samples.Add(grid_size / 2);
        while (spawn_samples.Count > 0)
        {
            int index = Random.Range(0,spawn_samples.Count);
            Vector2 current_spawn_sample = spawn_samples[index];
            bool rejected_sample = true;
            for (int i = 0; i < k; i++)
            {
                float angle_offset = Random.value * Mathf.PI * 2;
                float x = Mathf.Sin(angle_offset);
                float y = Mathf.Cos(angle_offset);
                Vector2 offset_direction = new Vector2(x, y);
                
                float new_magnitude = Random.Range(radius, 2 * radius);
                offset_direction *= new_magnitude;

                Vector2 sample = current_spawn_sample + offset_direction;
                if (is_valid(samples, grid, sample, grid_size, radius, cell_size))
                {
                    samples.Add(sample);
                    spawn_samples.Add(sample);
                    grid[(int)(sample.x / cell_size), (int)(sample.y / cell_size)] = samples.Count;
                    rejected_sample = false;
                    break;
                }
            }

            if (rejected_sample)
            {
                spawn_samples.RemoveAt(index);
            }
        }
        return samples;
    }

    void generateCannon()
    {
        int numCannonsToSpawn = Random.Range(1, numCannons);
        for (int i = 0; i < numCannonsToSpawn; i++)
        {
            int cannonToSpawn = Random.Range(0, SpawnPointsCannon.Count);
            Instantiate(cannon, SpawnPointsCannon[cannonToSpawn].position, SpawnPointsCannon[cannonToSpawn].rotation);
            SpawnPointsCannon.RemoveAt(cannonToSpawn);
        }
       
    }

}
