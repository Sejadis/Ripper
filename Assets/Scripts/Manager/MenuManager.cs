using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {


    public static MenuManager instance;

    [SerializeField]
    private GameObject defaultMenuPrefab;

    private GameObject activeMenuInstance;
    private GameObject activeMenuPrefab;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one MenuManager in the Scene!");
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        if (defaultMenuPrefab != null)
        {
            activeMenuInstance = Instantiate(defaultMenuPrefab);
            activeMenuPrefab = defaultMenuPrefab;
        }
    }

    void Update()
    {

    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public static void Exit()
    {
        Application.Quit();
    }

    public void SwitchMenu(GameObject newMenuPrefab)
    {
        if (newMenuPrefab != null)
        {
            activeMenuPrefab = newMenuPrefab;
            GameObject uiInstance = Instantiate(newMenuPrefab);
            uiInstance.gameObject.SetActive(true);
            Destroy(activeMenuInstance);
            activeMenuInstance = uiInstance;
        }
    }

}
