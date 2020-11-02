using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShow : MonoBehaviour
{
    public static UIShow instance;

    [Header("UI拖拽属性")]
    public GameObject[] dragObj;
    public GameObject PlaneOfMain;
    public GameObject PlaneOfSoldier;


    public List<GameObject> dragList;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void goToMain()
    {
        // TODO
        PlaneOfMain.SetActive(true);
        PlaneOfSoldier.SetActive(false);
        foreach (var now in dragList)
            DestroyImmediate(now);
        dragList.Clear();
    }

    public void goToSoldier()
    {
        PlaneOfMain.SetActive(false);
        PlaneOfSoldier.SetActive(true);
        foreach(var obj in dragObj)
        {
            GameObject now = Instantiate(obj);
            dragList.Add(now);
        }
    }
}
