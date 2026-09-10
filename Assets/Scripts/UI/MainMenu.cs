using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame(int scene)
    {
        GameManager.Instance.ShowMouse();
        SceneManager.LoadScene(scene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
