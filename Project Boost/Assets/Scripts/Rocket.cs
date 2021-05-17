using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    new Rigidbody rigidbody;
    
    //game variables
    [SerializeField] float thrustForce = 1000.0f;   //to control thrust of rocket


    [SerializeField] float speedRotation = 300.0f;    //to control rotation speed of rocket



    //Audios
    [SerializeField] AudioClip DeathSound;

    [SerializeField] AudioClip ThrustSound;

    [SerializeField] AudioClip LevelUpSound;

    //Effects
    [SerializeField] ParticleSystem DeathAnim;

    [SerializeField] ParticleSystem ThrustAnim;

    [SerializeField] ParticleSystem LevelUpAnim;


    new AudioSource audio;
    ParticleSystem effects;

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
        effects = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            if (state == State.Alive)
            {
                ThrustRocket();
                RotateRocket();
            }
        }
    }


    private void ThrustRocket()     //upward force function
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ForceUpwaards();
        }
        else
        {
            audio.Stop();
            ThrustAnim.Stop();
        }     
    }

    private void ForceUpwaards()
    {
        if (!audio.isPlaying)
        {
            audio.PlayOneShot(ThrustSound);
        }
        if (!effects.isPlaying)
        {
            ThrustAnim.Play();
        }
        rigidbody.AddRelativeForce(Vector3.up * thrustForce * Time.deltaTime);
    }

    private void RotateRocket()       //rotation of rocket function
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
        if (state != State.Alive) { return; }   //ignore collisions after death or after win

        switch (collision.gameObject.tag)
        {
            case "Enemy":
                state = State.Dead;
                if (audio.isPlaying) { audio.Stop(); }
                audio.PlayOneShot(DeathSound);
                print("Hit");
                Invoke("DeathCondition", 4f);
                break;

            case "Friendly":
                break;

            case "Finish":
                state = State.Transcending;
                if (audio.isPlaying) { audio.Stop(); }

                ProcessLevelUp();
                break;
        }
    }

    private void ProcessLevelUp()
    {
        if (currentLevel == maxLevel)
        {
            audio.PlayOneShot(LevelUpSound);
            Invoke("LevelUp", 3.5f);
        }
        else 
        {
            audio.PlayOneShot(LevelUpSound);
            Invoke("LevelUp", 3.5f);
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
            currentLevel++;
        }
    }
}
