using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [Header("Select Scene")]
    [SerializeField] public List<GameObject> listPlateform = new();
    private static UiManager instance;
    public bool SelectNbrPlayers = true;
    private GameObject _selectedButton;

    #region Singleton
    public static UiManager GetInstance()
    {
        if (instance)
        {
            return instance;
        }
        else
        {
            return instance = FindObjectOfType<UiManager>();
        }
    }
    #endregion

    public void Init()
    {
        if(SceneManager.GetActiveScene().name == "SelectMenu")
        {
            listPlateform.Add(GameObject.Find("Plat_P1"));
            listPlateform.Add(GameObject.Find("Plat_P2"));
            listPlateform.Add(GameObject.Find("Plat_P3"));
            listPlateform.Add(GameObject.Find("Plat_P4"));

            listPlateform[0].gameObject.SetActive(false);
            listPlateform[1].gameObject.SetActive(false);
            listPlateform[2].gameObject.SetActive(false);
            listPlateform[3].gameObject.SetActive(false);
        }
    }

    void Update()
    {
        
    }

}
