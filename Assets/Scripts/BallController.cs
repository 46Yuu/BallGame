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
        var particleSystemMain = GetComponent<ParticleSystem>().main;
        particleSystemMain.duration = rb.velocity.magnitude / 2;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rb.AddForce((transform.position - collision.gameObject.transform.position).normalized * collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude, ForceMode.Impulse);
            latesPlayerHit = collision.gameObject;
        }
    }
}
