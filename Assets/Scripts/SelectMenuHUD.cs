using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMenuHUD : MonoBehaviour
{
    public void SetTo2Players()
    {
        UIManager.GetInstance().listPlateform[0].SetActive(true);
        UIManager.GetInstance().listPlateform[1].SetActive(true);
        UIManager.GetInstance().listPlateform[2].SetActive(false);
        UIManager.GetInstance().listPlateform[3].SetActive(false);
        PlayerManager.Instance.playerInputMan.EnableJoining();
    }
    public void SetTo4Players()
    {
        UIManager.GetInstance().listPlateform[0].SetActive(true);
        UIManager.GetInstance().listPlateform[1].SetActive(true);
        UIManager.GetInstance().listPlateform[2].SetActive(true);
        UIManager.GetInstance().listPlateform[3].SetActive(true);
        PlayerManager.Instance.playerInputMan.EnableJoining();
    }
}
