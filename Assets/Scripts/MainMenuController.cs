using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void OptionButton()
    {

    }
    public void Restart()
    {
        GameController.GetInstance().Restart();
    }
    
    public void Menu()
    {
        foreach (PlayerInput player in PlayerManager.Instance._players)
        {
            Destroy(player.gameObject);
        }
        PlayerManager.Instance._players.Clear();
        SceneManager.LoadScene(0);
    }
    public void MainMenuButton()
    {
        SceneManager.LoadScene(0);
    }
}
