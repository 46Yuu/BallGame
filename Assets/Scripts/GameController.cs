using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public GameObject Ball;
    private GameObject startPosBall;

    private GameObject Player_1;
    private GameObject startPosP1;
    public int scoreP1 = 0;

    private GameObject Player_2;
    private GameObject startPosP2;
    public int scoreP2 = 0;

    public void Init()
    {
        Ball = GameObject.Find("Ball");
        Ball.layer = 8;
        Player_1 = GameObject.Find("Player_1");
        Player_2 = GameObject.Find("Player_2");
        startPosBall = GameObject.Find("startPosBall");
        startPosP1 = GameObject.Find("startPosP1");
        startPosP2 = GameObject.Find("startPosP2");
        RoundStart();
    }

    public void RoundStart()
    {
        Ball.transform.position = startPosBall.transform.position;
        Ball.GetComponent<Rigidbody>().velocity = Vector3.zero;

        Player_1.transform.position = startPosP1.transform.position;
        Player_1.transform.eulerAngles = new Vector3(0, 180, 0);
        Player_1.GetComponent<Rigidbody>().velocity = Vector3.zero;

        Player_2.transform.position = startPosP2.transform.position;
        Player_2.transform.eulerAngles = Vector3.zero;
        Player_2.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    public void CheckScore()
    {
        if(scoreP1 == scoreToWin ||scoreP2 == scoreToWin)
        {
            SceneManager.LoadScene(0);
            scoreP1 = 0;
            scoreP2 = 0;
        }

    }

    public GameObject GetBall()
    {
        return Ball;
    }
}
