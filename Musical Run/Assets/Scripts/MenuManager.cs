using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    
    public void LoadMusic(string musicName)
    {
        SceneManager.LoadScene(musicName);
    }
}
