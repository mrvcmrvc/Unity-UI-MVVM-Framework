using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class UIListener : MonoBehaviour
{
    protected virtual void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    protected virtual void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    protected abstract void OnSceneLoaded(Scene loadedScene, LoadSceneMode loadSceneMode);
    protected abstract void OnSceneUnloaded(Scene loadedScene);

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            UIMenuManager.Instance.OnBackPressed();
    }
}
