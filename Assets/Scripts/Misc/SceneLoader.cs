using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour 
{
    public void LoadScene (string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ReloadScene ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
