using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    new Rigidbody rigidbody;
    [SerializeField] float thrustForce = 1000.0f;   //to control thrust of rocket

    [SerializeField] float speedRotation = 300.0f;    //to control rotation speed of rocket


    new AudioSource audio;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ThrustRocket();
        MoveRocket();
    }

    private void ThrustRocket()     //upward force function
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (!audio.isPlaying)
            {
                audio.Play();
            }
            rigidbody.AddRelativeForce(Vector3.up * thrustForce * Time.deltaTime);
        }
        else
        {
            audio.Stop();
        }
    }

    private void MoveRocket()       //rotation of rocket function
    {
        rigidbody.freezeRotation = true;    //freeze autorotation of rocket before implementing manual rotation

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * speedRotation * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * speedRotation * Time.deltaTime);
        }

        rigidbody.freezeRotation = false;   //restore autorotaion after implementation of manual rotation is done
    }
}
