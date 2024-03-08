using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    private static GameController instance;

    public static GameController GetInstance()
    {
        if (instance)
        {
            return instance;
        }
        else
        {
            return instance = FindObjectOfType<GameController>();
        }
    }

    public int scoreToWin = 10;

    public int numberOfPlayers = 2;

    public string teamWinner;
    private GameObject winPanel;

    public GameObject Ball;
    private GameObject startPosBall;

    private GameObject Player_1;
    private GameObject Player_2;
    private GameObject Player_3;
    private GameObject Player_4;

    private GameObject startPosP1_2P;
    private GameObject startPosP2_2P;

    private GameObject startPosP1_4P;
    private GameObject startPosP2_4P;
    private GameObject startPosP3_4P;
    private GameObject startPosP4_4P;

    public int scoreP1 = 0;
    public int scoreP2 = 0;

    public void Init()
    {
        Ball = GameObject.Find("Ball");
        Ball.layer = 8;
        Player_1 = PlayerManager.Instance._players[0].gameObject;
        Player_2 = PlayerManager.Instance._players[1].gameObject;
        if (PlayerManager.Instance._playersRequired == 4)
        {
            Player_3 = PlayerManager.Instance._players[2].gameObject;
            Player_4 = PlayerManager.Instance._players[3].gameObject;
        }

        startPosBall = GameObject.Find("startPosBall");
        startPosP1_2P = GameObject.Find("startPosP1_2P");
        startPosP2_2P = GameObject.Find("startPosP2_2P");

        startPosP1_4P = GameObject.Find("startPosP2_2P");
        startPosP2_4P = GameObject.Find("startPosP2_2P");
        startPosP3_4P = GameObject.Find("startPosP2_2P");
        startPosP4_4P = GameObject.Find("startPosP2_2P");

        winPanel = GameObject.Find("WinPanel");
        winPanel.SetActive(false);

        RoundStart();
    }
    public void RoundStart()
    {
        Debug.Log("Round Start");
        Ball.transform.position = startPosBall.transform.position;
        Ball.GetComponent<Rigidbody>().velocity = Vector3.zero;

        if (PlayerManager.Instance._playersRequired == 2)
        {
            Player_1.transform.position = startPosP1_2P.transform.position;
            Player_1.transform.eulerAngles = new Vector3(0, 180, 0);
            Player_1.GetComponent<Rigidbody>().velocity = Vector3.zero;

            Player_2.transform.position = startPosP2_2P.transform.position;
            Player_2.transform.eulerAngles = Vector3.zero;
            Player_2.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        else if (PlayerManager.Instance._playersRequired == 4)
        {
            // TEAM 1
            Player_1.transform.position = startPosP1_4P.transform.position;
            Player_1.transform.eulerAngles = new Vector3(0, 180, 0);
            Player_1.GetComponent<Rigidbody>().velocity = Vector3.zero;

            Player_2.transform.position = startPosP2_4P.transform.position;
            Player_2.transform.eulerAngles = new Vector3(0, 180, 0);
            Player_2.GetComponent<Rigidbody>().velocity = Vector3.zero;


            //TEAM 2
            Player_3.transform.position = startPosP3_4P.transform.position;
            Player_3.transform.eulerAngles = Vector3.zero;
            Player_3.GetComponent<Rigidbody>().velocity = Vector3.zero;

            Player_4.transform.position = startPosP4_4P.transform.position;
            Player_4.transform.eulerAngles = Vector3.zero;
            Player_4.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    public bool CheckScore()
    {
        if (scoreP1 == scoreToWin || scoreP2 == scoreToWin)
        {
            if(scoreP1 > scoreP2)
            {
                teamWinner = "Team 1 Win";
            }
            else if (scoreP1 < scoreP2)
            {
                teamWinner = "Team 2 Win";
            }   
            else if (scoreP1 == scoreP2)
            {
                teamWinner = "Draw";
            }
            EndGame();
            return true;
        }
        return false;
    }
    private void EndGame()
    {
        Time.timeScale = 0;
        winPanel.SetActive(true);
        winPanel.GetComponentInChildren<TMPro.TMP_Text>().text = teamWinner ;
        UIManager.GetInstance().eventSystem.SetSelectedGameObject(winPanel.transform.Find("Restart").gameObject);
        foreach(PlayerInput player in PlayerManager.Instance._players)
        {
            player.gameObject.GetComponent<PlayerController>().energySlider.gameObject.SetActive(false);
        }
    }
    public GameObject GetBall()
    {
        return Ball;
    }
}
