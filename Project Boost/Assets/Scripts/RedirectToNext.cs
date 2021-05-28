using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RedirectToNext : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Invoke("NextScene", 9f);
    }
    private void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
