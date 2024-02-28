using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonScript : MonoBehaviour
{
    public GameObject ball;
    private Transform SpawnPoint;

    void Awake()
    {
        SpawnPoint = transform.Find("BulletSpawn");
    }

    void Start()
    {
        StartCoroutine(DelayShoot());
    }

    private IEnumerator DelayShoot()
    {
        bool running = true;
        while(running)
        {
            yield return new WaitForSeconds(5f);
            ShootBall();
        }
    }
    
    void ShootBall(){
        GameObject temp = Instantiate(ball, SpawnPoint.position, Quaternion.identity);
        temp.GetComponent<Rigidbody>().AddForce(Vector3.forward * 1000);
    }
}
