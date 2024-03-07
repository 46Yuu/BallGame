using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
