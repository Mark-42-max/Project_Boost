using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    new Rigidbody rigidbody;
    public Vector3 force;
    public float speed = 5.0f;


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
            print("Thrust");
        }
    }

    private void MoveRocket()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * speed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * speed);
        }
    }
}
