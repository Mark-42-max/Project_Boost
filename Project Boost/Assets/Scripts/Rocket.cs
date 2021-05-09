using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    new Rigidbody rigidbody;
    [SerializeField] float thrustForce = 1000.0f;   //to control thrust of rocket

    [SerializeField] float speedRotation = 300.0f;    //to control rotation speed of rocket


    new AudioSource audio;

    private static int currentLevel = 1;
    readonly private int maxLevel = 2;

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

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Enemy":
                currentLevel = 1;
                SceneManager.LoadScene(0);
                break;

            case "Friendly":
                break;

            case "Finish":
                if (currentLevel == maxLevel)
                {
                    currentLevel = 1;
                    SceneManager.LoadScene(0);
                }
                else if (currentLevel < maxLevel)
                {
                    currentLevel++;
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }

                break;
        }
    }
}
