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

    //level data
    private static int currentLevel = 1;
    readonly private int maxLevel = 2;

    enum State { Dead, Alive, Transcending}
    State state = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            ThrustRocket();
            MoveRocket();
        }
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
        if (state != State.Alive) { return; }

        switch (collision.gameObject.tag)
        {
            case "Enemy":
                state = State.Dead;
                if (audio.isPlaying) { audio.Stop(); }
                print("Hit");
                Invoke("DeathCondition", 2f);
                break;

            case "Friendly":
                break;

            case "Finish":
                state = State.Transcending;
                if (audio.isPlaying) { audio.Stop(); }
                Invoke("LevelUp", 2f);
                break;
        }
    }

    private void DeathCondition()
    {
        SceneManager.LoadScene(currentLevel);
    }

    private void LevelUp()
    {
        if (currentLevel == maxLevel)
        {
            SceneManager.LoadScene(currentLevel + 1);
        }
        else if (currentLevel < maxLevel)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            rigidbody.isKinematic = true;
            currentLevel++;
        }
    }
}
