using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    private void Start()
    {
        AdsManager.Instance.HideBanner();
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void ActivateObject(GameObject obj)
    {
        obj.SetActive(true);
    }

    public void DeActivateObject(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void SetPause(bool paused)
    {
        GameSettings.Instance.SetPaused(paused);
    }

    public void ContinuePreviousGame(bool continueGame)
    {
        GameSettings.Instance.SetContinuePreviousGame(continueGame);
    }

    public void ExitGameWonOrOver()
    {
        ExitToMenu();
        GameSettings.Instance.SetContinuePreviousGame(false);
    }

    public void ExitToMenu()
    {
        GameEvents.OnExitToMenuMethod();
        GameSettings.Instance.SetPaused(false);
        GameSettings.Instance.SetContinuePreviousGame(true);
        SceneManager.LoadScene("MainMenu");
    }
}
