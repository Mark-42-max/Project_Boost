using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Controls()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Fuel()
    {
        SceneManager.LoadScene(8);
    }

    public void Obstacle()
    {
        SceneManager.LoadScene(6);
    }
}
