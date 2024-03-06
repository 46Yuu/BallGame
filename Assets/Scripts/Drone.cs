using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;

public class Drone : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float chaseSpeed;
    List<Vector3> path;
    int currentPoint = 0;
    int nextPoint = 1;

    [SerializeField] private float timer;
    private bool isBallGrabbed = false;
    private bool isChasing = false;

    [SerializeField] private GameObject ball;

    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(FindAnyObjectByType<MeshGenerator>().initialSeed);
        SetupPath();
        transform.position = path[0];
        gameObject.layer = 8;
        StartChaseTimer();
    }

    // Update is called once per frame
    void Update()
    {
        float timeSpeed = speed * Time.deltaTime;
        if (transform.position == path[nextPoint])
        {
            if(isBallGrabbed)
            {
                ball.transform.parent = null;
                ball.GetComponent<Rigidbody>().isKinematic = false;
                ball.GetComponent<Rigidbody>().useGravity = true;
                isBallGrabbed = false;
                StartChaseTimer();
            }
            currentPoint = nextPoint;
            nextPoint++;
        }
        if (nextPoint >= path.Count)
        {
            nextPoint = 0;
        }
        if(isChasing)
        {
            timeSpeed = chaseSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, ball.transform.position, timeSpeed);
            transform.rotation = Quaternion.LookRotation(ball.transform.position - transform.position);
        }
        else
        {
            if(isBallGrabbed)
            {
                timeSpeed = chaseSpeed * Time.deltaTime;
            } 
            transform.position = Vector3.MoveTowards(transform.position, path[nextPoint], timeSpeed);
            transform.rotation = Quaternion.LookRotation(path[nextPoint] - transform.position);
        }
            
    }
    IEnumerator Chase()
    {
        yield return new WaitForSeconds(timer);
        isChasing = true;

    }

    public void StartChaseTimer()
    {
        timer = Random.Range(30, 60);
        StartCoroutine(Chase());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball") && isChasing && !isBallGrabbed)
        {
            isBallGrabbed = true;
            isChasing = false;
            collision.transform.parent = transform;
            collision.transform.position = transform.position;
            collision.transform.position += new Vector3(0, 5, 0);
            collision.rigidbody.isKinematic = true;
            collision.rigidbody.useGravity = false;
        }
    }

    private void SetupPath()
    {
        path = new List<Vector3>();
        int numCheckPoints = Random.Range(4, 6);
        int xMinField = -(FindAnyObjectByType<MeshGenerator>().xSize / 2);
        int xMaxField = FindAnyObjectByType<MeshGenerator>().xSize / 2;
        int zMinField = -(FindAnyObjectByType<MeshGenerator>().zSize / 2);
        int zMaxField = FindAnyObjectByType<MeshGenerator>().zSize / 2; 
        for(int i = 0; i < numCheckPoints; i++)
        {
            path.Add(new Vector3(Random.Range(xMinField+5,xMaxField-5), FindAnyObjectByType<MeshGenerator>().maxTerrainHeight + 3, Random.Range(zMinField+15,zMaxField-15)));
        }    
    }
}
