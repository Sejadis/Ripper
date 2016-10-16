using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneButton : MonoBehaviour {

    public void LoadScene(int levelIndex)
    {
        LoadingScreen.LoadScene(levelIndex);
    }

    public void LoadScene(string levelName)
    {
        int levelIndex = SceneManager.GetSceneByName(levelName).buildIndex;
        LoadingScreen.LoadScene(levelIndex);
    }

}
