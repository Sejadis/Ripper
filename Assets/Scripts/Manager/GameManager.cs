using UnityEngine;
using System.Collections;
using System;

public class GameManager : MonoBehaviour {

    public static GameManager instance;


    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one GameManager in the Scene!");
        }
        else
        {
            instance = this;
        }
    }

    void OnEnable()
    {
        EventHandler.OnPause += OnPause;
        EventHandler.OnResume += OnResume;
    }

    void OnDisable()
    {
        EventHandler.OnPause -= OnPause;
        EventHandler.OnResume -= OnResume;
    }

    private void OnPause()
    {
        Time.timeScale = 0;
    }

    private void OnResume()
    {
        Time.timeScale = 1;
    }

}
