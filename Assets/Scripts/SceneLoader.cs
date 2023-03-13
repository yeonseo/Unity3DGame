using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private AssetBundle myLoadedAssetBundle;
    private string[] scenePaths;

    // Use this for initialization
    void Start()
    {
        // myLoadedAssetBundle = AssetBundle.LoadFromFile("Assets/AssetBundles/scenes");
        // scenePaths = myLoadedAssetBundle.GetAllScenePaths();
    }

    public void LoadNameScene(string scenename)
    {
        Debug.Log("sceneName to load: " + scenename);
        SceneManager.LoadScene(scenename);
    }

    public void LoadIndexScene(int sceneANumber)
    {
        Debug.Log("sceneBuildIndex to load: " + sceneANumber);
        SceneManager.LoadScene(sceneANumber);
    }

    public void LoadNextScene()
    {
        int current = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(current + 1);
    }

    public void LoadStartScene() {
        SceneManager.LoadScene(0);
    }
}
