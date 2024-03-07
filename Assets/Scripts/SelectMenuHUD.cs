using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectMenuHUD : MonoBehaviour
{
    public void SetTo2Players()
    {
        UIManager.GetInstance().listPlateform[0].SetActive(true);
        UIManager.GetInstance().listPlateform[1].SetActive(true);
        UIManager.GetInstance().listPlateform[2].SetActive(false);
        UIManager.GetInstance().listPlateform[3].SetActive(false);
        PlayerManager.Instance.playerInputMan.EnableJoining();
        UIManager.GetInstance().canClickButton = false;
        PlayerManager.Instance._playersRequired = 2;
        if(PlayerManager.Instance._players.Count > 2)
        {
            Destroy(PlayerManager.Instance._players[2].gameObject);
            Destroy(PlayerManager.Instance._players[3].gameObject);
            PlayerManager.Instance._players.Remove(PlayerManager.Instance._players[2]);
            PlayerManager.Instance._players.Remove(PlayerManager.Instance._players[3]);
        }
    }
    public void SetTo4Players()
    {
        UIManager.GetInstance().listPlateform[0].SetActive(true);
        UIManager.GetInstance().listPlateform[1].SetActive(true);
        UIManager.GetInstance().listPlateform[2].SetActive(true);
        UIManager.GetInstance().listPlateform[3].SetActive(true);
        PlayerManager.Instance.playerInputMan.EnableJoining();
        UIManager.GetInstance().canClickButton = false;
        PlayerManager.Instance._playersRequired = 4;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
