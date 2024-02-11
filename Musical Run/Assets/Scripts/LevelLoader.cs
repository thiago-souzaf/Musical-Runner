using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] GameObject go_LoadingScreen;
    [SerializeField] Slider progressBar;
    [SerializeField] TextMeshProUGUI percentageText;
    public void LoadLevel(string levelName)
    {
        StartCoroutine(LoadAsync(levelName));
    }

    public void ReloadLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        StartCoroutine(LoadAsync(currentSceneName));
    }

    private IEnumerator LoadAsync(string levelName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelName);

        go_LoadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            progressBar.value = progress;
            percentageText.text = progress * 100 + "%";

            yield return null;
        }
    }
}
