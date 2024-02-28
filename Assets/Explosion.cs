using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private int exploForce;
    private int minRadius;
    private int maxRadius;
    private Collider sphereCollider;

    // Start is called before the first frame update
    void Awake()
    {
        exploForce = 1000;
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<Rigidbody>().AddForce((other.transform.position - transform.position).normalized * exploForce, ForceMode.Impulse);
        }
    }
}
