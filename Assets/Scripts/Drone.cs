using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float chaseSpeed;
    List<Vector3> path = new List<Vector3>();
    int currentPoint = 0;
    int nextPoint = 1;

    [SerializeField] private float timer;
    private bool isBallGrabbed = false;
    private bool isChasing = false;

    private GameObject ball;

    // Start is called before the first frame update
    void Start()
    {
        ball = GameObject.Find("Ball");
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("DroneRoute"))
        {
            path.Add(go.transform.position);
        }
        transform.position = path[0];
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
        }
        else
        {
            if(isBallGrabbed)
            {
                timeSpeed = chaseSpeed * Time.deltaTime;
            } 
            transform.position = Vector3.MoveTowards(transform.position, path[nextPoint], timeSpeed);
        }
            
    }
    IEnumerator Chase()
    {
        yield return new WaitForSeconds(timer);
        isChasing = true;

    }

    public void StartChaseTimer()
    {
        StartCoroutine(Chase());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball") && isChasing && !isBallGrabbed)
        {
            isBallGrabbed = true;
            isChasing = false;
            other.transform.parent = transform;
            other.transform.position = transform.position;
            other.transform.position += new Vector3(0, 5, 0);
            other.GetComponent<Rigidbody>().isKinematic = true;
            other.GetComponent<Rigidbody>().useGravity = false;
            //other.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
