using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelLoadManager : MonoBehaviour {

    public static LevelLoadManager instance;

    [SerializeField]
    GameObject loadingScreenPrefab;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one LevelLoadManager in the Scene!");
        }
        else
        {
            instance = this;
        }
    }
    public void LoadWithLoadingScreen(string scene)
    {
        if(loadingScreenPrefab != null)
        {
            Instantiate(loadingScreenPrefab);
            SceneManager.LoadScene(scene, LoadSceneMode.Additive);
             
        }
    } 
}
