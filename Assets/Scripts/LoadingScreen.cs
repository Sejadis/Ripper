using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour {
    [Header("Visuals")]
    [SerializeField]
    Image loadingIcon;
    [SerializeField]
    Image finishedLoadingIcon;
    [SerializeField]
    Image progressBar;
    [SerializeField]
    Text loadingText;

    [Header("Loading Settings")]
    [SerializeField]
    LoadSceneMode loadSceneMode = LoadSceneMode.Single;
    [SerializeField]
    ThreadPriority loadThreadPriority;

    [Header("Other Settings")]
    [SerializeField]
    private float minLoadingTime = 5f;

    AsyncOperation operation;
    Scene currentScene;

    static int sceneToLoad = -1;
    static int loadingSceneIndex = 1;


    public static void LoadScene(int levelNum)
    {
        Application.backgroundLoadingPriority = ThreadPriority.High;
        sceneToLoad = levelNum;
        SceneManager.LoadScene(loadingSceneIndex);
    }

    public static void LoadScene(string levelName)
    {
        Application.backgroundLoadingPriority = ThreadPriority.High;
        sceneToLoad = SceneManager.GetSceneByName(levelName).buildIndex;
        SceneManager.LoadScene(loadingSceneIndex);
    }

    void Start()
    {
        if (sceneToLoad < 0)
            return;

        currentScene = SceneManager.GetActiveScene();
        StartCoroutine(LoadAsync(sceneToLoad));
    }

    private IEnumerator LoadAsync(int levelNum)
    {
        float startLoadingTime = Time.time;
        float endLoadingTime = startLoadingTime + minLoadingTime;
        float keepLoadingActiveTime;

        ShowLoadingVisuals();

        yield return null;

        StartOperation(levelNum);

        float lastProgress = 0f;

        // operation does not auto-activate scene, so it's stuck at 0.9
        while (DoneLoading() == false)
        {
            yield return null;

            if (Mathf.Approximately(operation.progress, lastProgress) == false)
            {
                progressBar.fillAmount = operation.progress;
                lastProgress = operation.progress;
            }
        }

        ShowCompletionVisuals();

        if(endLoadingTime > Time.time)
        {
            keepLoadingActiveTime = endLoadingTime - Time.time;
            Debug.Log("Loading started at: " + startLoadingTime);
            Debug.Log("Loading finished at: " + Time.time);
            Debug.Log("Loading should finish at: " + endLoadingTime);
            Debug.Log("Keeping Loading active for " + keepLoadingActiveTime + " more seconds.");

            yield return new WaitForSeconds(keepLoadingActiveTime);
        }
        /*
                yield return new WaitForSeconds(fadeDuration);
        */
        if (loadSceneMode == LoadSceneMode.Additive)
            SceneManager.UnloadScene(currentScene.name);
        else
            operation.allowSceneActivation = true;
    }

    private void StartOperation(int levelNum)
    {
        Application.backgroundLoadingPriority = loadThreadPriority;
        operation = SceneManager.LoadSceneAsync(levelNum, loadSceneMode);


        if (loadSceneMode == LoadSceneMode.Single)
            operation.allowSceneActivation = false;
    }

    private bool DoneLoading()
    {
        return (loadSceneMode == LoadSceneMode.Additive && operation.isDone) || (loadSceneMode == LoadSceneMode.Single && operation.progress >= 0.9f);
    }


    void ShowLoadingVisuals()
    {
        loadingIcon.gameObject.SetActive(true);
        finishedLoadingIcon.gameObject.SetActive(false);

        progressBar.fillAmount = 0f;
//        loadingText.text = "LOADING...";
    }

    void ShowCompletionVisuals()
    {
        loadingIcon.gameObject.SetActive(false);
        finishedLoadingIcon.gameObject.SetActive(true);

        progressBar.fillAmount = 1f;
        loadingText.text = "";
    }

}
