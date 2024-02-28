using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody rb;
    public GameObject latesPlayerHit;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rb.AddForce((collision.gameObject.transform.position - transform.position).normalized * collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude, ForceMode.Impulse);
            latesPlayerHit = collision.gameObject;
        }
    }
}
