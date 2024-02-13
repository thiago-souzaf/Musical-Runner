using UnityEngine;

public class PauseController : MonoBehaviour
{
    [SerializeField] GameObject PauseMenu;

    private void Start()
    {
        PauseMenu.SetActive(false);
    }

    public void PauseGame()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void ContinueGame()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

}
