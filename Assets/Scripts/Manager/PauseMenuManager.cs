using UnityEngine;
using System.Collections;
using System;

public class PauseMenuManager : MonoBehaviour {

    public static PauseMenuManager instance;

    [SerializeField]
    private GameObject pauseMenu;

    private GameObject pauseMenuInstance;
//    private bool pauseMenuActive = false;
    private bool isPaused = false;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one PauseMenuManager in the Scene!");
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

    private void OnResume()
    {
        Destroy(pauseMenuInstance);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isPaused = false;
    }

    private void OnPause()
    {
        pauseMenuInstance = Instantiate(pauseMenu);

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        isPaused = true;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (InputManager.GetKeyDown("Pause"))
        {
            if (isPaused)
            {
                EventHandler.TriggerOnResume();
            }
            else
            {
                EventHandler.TriggerOnPause();
            }
        }

    }

    /*
    void ToggleMenu()
    {
        if (pauseMenuActive)
        {
            Time.timeScale = 1;



            pauseMenuActive = false;
        }
        else
        {
            Time.timeScale = 0;



            pauseMenuActive = true;
        }
    }

    public static void TogglePauseMenu()
    {
        instance.ToggleMenu();
    }*/
}
