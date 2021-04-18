using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    new Rigidbody rigidbody;
    public float force = 1000.0f;
    public float speed = 300.0f;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ThrustRocket();
        MoveRocket();
    }

    private void ThrustRocket()
    {
        if (Input.GetKey(KeyCode.Space))
        {

            rigidbody.AddRelativeForce(Vector3.up * force * Time.deltaTime);
        }
    }

    private void MoveRocket()
    {
        rigidbody.freezeRotation = true;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * speed * Time.deltaTime);
        }

        rigidbody.freezeRotation = false;
    }
}
