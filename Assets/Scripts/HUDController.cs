using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public TextMeshPro scoreP1;
    public TextMeshPro scoreP2;
    // Start is called before the first frame update
    void Awake()
    {
        scoreP1 = GameObject.Find("ScoreTeam1").GetComponent<TextMeshPro>();
        scoreP2 = GameObject.Find("ScoreTeam2").GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
