using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public EventSystem eventSystem;
    [SerializeField] private GameObject _4playerBtn;
    [SerializeField] private GameObject _2playerBtn;
    [SerializeField] private GameObject startGameBtn;
    public Button activeButton;
    public bool canClickButton = true;

    public List<GameObject> listPlateform = new List<GameObject>();

    #region Singleton
    public static UIManager GetInstance()
    {
        if (instance)
            return instance;
        else return instance = GameObject.FindObjectOfType<UIManager>();
    }
    #endregion
    // Start is called before the first frame update
    public void Init()
    {
        listPlateform.Add(GameObject.Find("Plat_P1"));
        listPlateform.Add(GameObject.Find("Plat_P2"));
        listPlateform.Add(GameObject.Find("Plat_P3"));
        listPlateform.Add(GameObject.Find("Plat_P4"));
        eventSystem = FindAnyObjectByType<EventSystem>();
        eventSystem.SetSelectedGameObject(_2playerBtn);
        startGameBtn = GameObject.Find("StartGame");
        startGameBtn.GetComponent<Button>().interactable = false;
        foreach (GameObject plat in listPlateform)
        {
            plat.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "SelectScene")
        {
            if (PlayerManager.Instance._players.Count == PlayerManager.Instance._playersRequired)
            {
                eventSystem.SetSelectedGameObject(startGameBtn);
                startGameBtn.GetComponent<Button>().interactable = true;
            }
            else
            {
                startGameBtn.GetComponent<Button>().interactable = false;
            }
        }
    }
}
