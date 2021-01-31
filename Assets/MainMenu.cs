using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject gamePanel;
    [SerializeField] GameObject mainMenuPanel;

    public void LoadGamePanel()
    {
        gamePanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    public void LoadMainMenuPanel()
    {
        mainMenuPanel.SetActive(true);
        gamePanel.SetActive(false);
    }

    public void LoadOptionsPanel()
    {

    }

    public void OfflineLobby()
    {
        SceneManager.LoadScene(1);
    }

    public void OnlineLobby()
    {
        NetworkManagerCustom.Instance.ConnectToRelay();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
