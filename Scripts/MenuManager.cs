using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public AudioSource audio;
    public void Play()
    {
        audio.Stop();
        SceneManager.LoadScene(1);

    }

    public void Exit()
    {
        Application.Quit();
    }
}
