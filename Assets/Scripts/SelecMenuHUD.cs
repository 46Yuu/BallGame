using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelecMenuHUD : MonoBehaviour
{
    public void Select2Players()
    {
        UiManager.GetInstance().listPlateform[0].gameObject.SetActive(true);
        UiManager.GetInstance().listPlateform[1].gameObject.SetActive(true);
        UiManager.GetInstance().SelectNbrPlayers = false;
        PlayerManager.Instance._playerInputMan.EnableJoining();
    }
    public void Select4Players()
    {
        UiManager.GetInstance().listPlateform[0].gameObject.SetActive(true);
        UiManager.GetInstance().listPlateform[1].gameObject.SetActive(true);
        UiManager.GetInstance().listPlateform[2].gameObject.SetActive(true);
        UiManager.GetInstance().listPlateform[3].gameObject.SetActive(true);
        UiManager.GetInstance().SelectNbrPlayers = false;
        PlayerManager.Instance._playerInputMan.EnableJoining();
    }
}
