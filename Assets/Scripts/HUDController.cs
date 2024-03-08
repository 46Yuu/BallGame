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
    private GameObject sliderP1;
    private GameObject sliderP2;
    private GameObject sliderP3;
    private GameObject sliderP4;
    // Start is called before the first frame update
    void Awake()
    {
        scoreP1 = GameObject.Find("ScoreTeam1");
        scoreP2 = GameObject.Find("ScoreTeam2");
        textGoal = GameObject.Find("TextGoal");
        sliderP1 = GameObject.FindGameObjectWithTag("EnergyP1");
        sliderP2 = GameObject.FindGameObjectWithTag("EnergyP2");
        sliderP3 = GameObject.FindGameObjectWithTag("EnergyP3");
        sliderP4 = GameObject.FindGameObjectWithTag("EnergyP4");
        if (PlayerManager.Instance._playersRequired == 2)
        {
            sliderP3.SetActive(false);
            sliderP4.SetActive(false);
            sliderP1.transform.position = sliderP3.transform.position;
            sliderP2.transform.position = sliderP4.transform.position;
        }

    }
    private void Start()
    {
        textGoal.gameObject.SetActive(false);
    }
}
