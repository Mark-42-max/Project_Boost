using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;


public class Rocket : MonoBehaviour
{
    new Rigidbody rigidbody;

    private Canvas handEnv;
    
    //game variables
    [SerializeField] float thrustForce = 1000.0f;   //to control thrust of rocket


    [SerializeField] float speedRotation = 300.0f;    //to control rotation speed of rocket


    [SerializeField] float levelLoadDelayDeath = 4f;   //Load the level after death

    [SerializeField] float levelLoadDelayWin = 3.5f;   //Load the level after win


    //Audios
    [SerializeField] AudioClip DeathSound;

    [SerializeField] AudioClip ThrustSound;

    [SerializeField] AudioClip LevelUpSound;

    //Effects
    [SerializeField] ParticleSystem DeathAnim;

    [SerializeField] ParticleSystem ThrustAnim;

    [SerializeField] ParticleSystem LevelUpAnim;

    //Developer tools
    private bool collisionDisabled = false;

    //Fuel System
    public Text fuelDisplay;
    [SerializeField] private float fuelData = 1000;


    new AudioSource audio;

    //level data
    public static int currentLevel = 1;
    readonly private int maxLevel = 10;

    enum State { Dead, Alive, Transcending}
    State state = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
        handEnv = GameObject.FindGameObjectWithTag("EditorOnly").GetComponent<Canvas>();
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            handEnv.enabled = false;
        }
        if(SystemInfo.deviceType == DeviceType.Handheld)
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
        rigidbody = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
        fuelDisplay.text = "Fuel: " + fuelData.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (SystemInfo.deviceType == DeviceType.Desktop)    //for desktop users
        {
            if (state == State.Alive)
            {
                ThrustRocket();
                RotateRocket();
            }
        }

        else if (SystemInfo.deviceType == DeviceType.Handheld)  //for android users
        {
            if (state == State.Alive)
            {
                HandheldThrust();
                HandheldRotate();
            }
        }
        if (Debug.isDebugBuild)
        {
            DeveloperControls();
        }

    }

    private void DeveloperControls()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled;
        }
        if (Input.GetKey(KeyCode.L))
        {
            LevelUp();
        }
    }

    private void HandheldThrust()
    {
        if (CrossPlatformInputManager.GetButton("Thrust"))
        {
            ForceUpwards();
        }
        else
        {
            audio.Stop();
            ThrustAnim.Stop();
        }
    }

    private void HandheldRotate()
    {
        rigidbody.freezeRotation = true;
        if (CrossPlatformInputManager.GetButton("Left"))
        {
            transform.Rotate(Vector3.forward * speedRotation * Time.deltaTime);
        }
        else if (CrossPlatformInputManager.GetButton("Right"))
        {
            transform.Rotate(-Vector3.forward * speedRotation * Time.deltaTime);
        }
        rigidbody.freezeRotation = false;
    }

    private void ThrustRocket()     //upward force function
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ForceUpwards();
        }
        else
        {
            audio.Stop();
            ThrustAnim.Stop();
        }     
    }

    private void ForceUpwards()
    {
        rigidbody.AddRelativeForce(Vector3.up * thrustForce * Time.deltaTime);
        if (!audio.isPlaying)
        {
            audio.PlayOneShot(ThrustSound);
        }
        if (!ThrustAnim.isPlaying)
        {
            ThrustAnim.Play();
        }
        Fuelling();
    }

    private void Fuelling()
    {
        if(fuelData <=Mathf.Epsilon) 
        {
            fuelDisplay.text = "Fuel: 0";
            state = State.Dead;
            DeathAudioVisual();
            Invoke("DeathCondition", levelLoadDelayDeath);
        }
        fuelData -= Time.deltaTime;
        print(fuelData);
        fuelDisplay.text = "Fuel: " + ((int)fuelData).ToString();
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
        if (state != State.Alive || collisionDisabled) { return; }   //ignore collisions after death or after win

        switch (collision.gameObject.tag)
        {
            case "Enemy":
                state = State.Dead;
                DeathAudioVisual();
                Invoke("DeathCondition", levelLoadDelayDeath);
                break;

            case "Friendly":
                if(state == State.Alive)
                {
                    //do nothing
                }
                break;

            case "Finish":
                state = State.Transcending;
                LevelUpAudioVisual();
                LevelUpAnim.Play();

                ProcessLevelUp();
                break;
        }
    }

    private void LevelUpAudioVisual()
    {
        if (audio.isPlaying) { audio.Stop(); }
        if (ThrustAnim.isPlaying) { ThrustAnim.Stop(); }
    }

    private void DeathAudioVisual()
    {
        if (audio.isPlaying) { audio.Stop(); }
        if (ThrustAnim.isPlaying) { ThrustAnim.Stop(); }
        audio.PlayOneShot(DeathSound);
        print("Hit");
        DeathAnim.Play();
    }

    private void ProcessLevelUp()
    {
        if (currentLevel == maxLevel)
        {
            audio.PlayOneShot(LevelUpSound);
            Invoke("LevelUp", levelLoadDelayWin);
        }
        else 
        {
            audio.PlayOneShot(LevelUpSound);
            Invoke("LevelUp", levelLoadDelayWin);
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
            SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 2);
        }
        else if (currentLevel < maxLevel)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            currentLevel++;
        }
    }
}
