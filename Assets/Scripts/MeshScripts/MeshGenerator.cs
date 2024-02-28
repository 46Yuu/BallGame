using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;

    UnityEngine.Vector3[] vertices;
    int[] triangles;
    Color[] colors;

    public int xSize;
    public int zSize;

    public int initialSeed;
    public float intSeeded;

    private float minTerrainHeight;
    private float maxTerrainHeight;
    public Gradient gradient;
    // Start is called before the first frame update
    void Awake()
    {
        initialSeed = Random.Range(0,999);
        Random.InitState(initialSeed);
        intSeeded = Random.Range(1f,1000f);
        mesh = new Mesh();
        CreateShape();   
        UpdateMesh();
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;

    }


    void CreateShape(){
        vertices = new UnityEngine.Vector3[(xSize+1) * (zSize+1)];
        for (int i = 0,z = 0; z <= zSize ; z++)
        {
            for(int x = 0; x <= xSize ; x++)
            {
                float y = Mathf.PerlinNoise(intSeeded+x*0.05f,intSeeded+z*0.05f) *5f;
                vertices[i] = new UnityEngine.Vector3(x,y,z);

                if(y > maxTerrainHeight)
                    maxTerrainHeight = y;
                if(y < minTerrainHeight)
                    minTerrainHeight = y;
                i++;

            }
        }

        triangles = new int[xSize * zSize * 6];
        int vert = 0;
        int tris = 0;
        
        for(int z = 0; z < zSize ; z++)
        {
            for(int x = 0;x <xSize;x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;
                vert++;
                tris+=6;
            }
            vert++;
        }

        colors = new Color[vertices.Length];

        for(int i = 0, z = 0; z <= zSize; z++)
        {
            for(int x = 0; x<= xSize; x++)
            {
                float height = Mathf.InverseLerp(minTerrainHeight,maxTerrainHeight,vertices[i].y);
                colors[i] = gradient.Evaluate(height);
                i++;
            }
        }
    }

    
    void UpdateMesh()
    {
        mesh.Clear();
        
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;

        mesh.RecalculateNormals();
    }
}
