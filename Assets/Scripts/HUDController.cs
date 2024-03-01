using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    private static HUDController instance;
    public static HUDController GetInstance()
    {
        if (instance)
        {
            return instance;
        }
        else
        {
            return instance = FindObjectOfType<HUDController>();
        }
    }

    public GameObject scoreP1;
    public GameObject scoreP2;
    public GameObject textGoal;
    // Start is called before the first frame update
    void Awake()
    {
        scoreP1 = GameObject.Find("ScoreTeam1");
        scoreP2 = GameObject.Find("ScoreTeam2");
        textGoal = GameObject.Find("TextGoal");
    }
    private void Start()
    {
        textGoal.gameObject.SetActive(false);
    }
}
