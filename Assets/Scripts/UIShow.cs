using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShow : MonoBehaviour
{
    public static UIShow instance;

    [Header("UI拖拽属性")]
    public GameObject[] dragSoldierObj;
    public GameObject[] dragCastleObj;

    public GameObject PlaneOfMain;
    public GameObject PlaneOfSoldier;
    public GameObject PlaneOfCastle;

    public RawImage red;

    public List<GameObject> dragSoldierList;
    public List<GameObject> dragCastleList;

    void Awake()
    {
        instance = this;
        isTopCam = true;
    }

    void Start()
    {

    }

    int widthOfRed = 1080 / 2;
    int lastWidthOfRed;
    void Update()
    {
        foreach (var now in dragSoldierList)
        {
            string nameOfSoldier = now.GetComponent<Drag>().nameOfUnit;
            var s = UnitManager.instance.dicUnit[nameOfSoldier];
            if (s.needFood > User.instance.foodVal || s.needMineral > User.instance.mineralVal)
                now.SetActive(false);
            else
                now.SetActive(true);
        }

        foreach (var now in dragCastleList)
        {
            string nameOfCastle = now.GetComponent<Drag>().nameOfUnit;
            var s = UnitManager.instance.dicUnit[nameOfCastle];
            if (s.needFood > User.instance.foodVal || s.needMineral > User.instance.mineralVal)
                now.SetActive(false);
            else
                now.SetActive(true);
        }

        widthOfRed = 1080 * User.instance.lifeVal / (User.instance.lifeVal + Computer.instance.lifeVal);

        if (widthOfRed == lastWidthOfRed) return;

        red.GetComponent<RectTransform>().sizeDelta = new Vector2(widthOfRed, 0);
        red.GetComponent<RectTransform>().anchoredPosition = new Vector2(widthOfRed / 2, 0);

        lastWidthOfRed = widthOfRed;

    }

    public void goToMain()
    {
        // TODO
        PlaneOfMain.SetActive(true);
        PlaneOfSoldier.SetActive(false);
        PlaneOfCastle.SetActive(false);
        foreach (var now in dragSoldierList)
            DestroyImmediate(now);
        dragSoldierList.Clear();

        foreach (var now in dragCastleList)
            DestroyImmediate(now);
        dragCastleList.Clear();
    }

    public void goToSoldier()
    {
        PlaneOfMain.SetActive(false);
        PlaneOfSoldier.SetActive(true);
        PlaneOfCastle.SetActive(false);
        foreach (var obj in dragSoldierObj)
        {
            GameObject now = Instantiate(obj);
            dragSoldierList.Add(now);
        }
    }

    public void goToCastle()
    {
        PlaneOfMain.SetActive(false);
        PlaneOfSoldier.SetActive(false);
        PlaneOfCastle.SetActive(true);
        foreach (var obj in dragCastleObj)
        {
            GameObject now = Instantiate(obj);
            dragCastleList.Add(now);
        }
    }

    public bool isTopCam;
    public GameObject topCam;
    public GameObject downCam;
    public Button soldierBtn;
    public Button castleBtn;
    public void switchCam()
    {
        isTopCam = !isTopCam;
        topCam.SetActive(isTopCam);
        downCam.SetActive(!isTopCam);
        soldierBtn.enabled = isTopCam;
        castleBtn.enabled = isTopCam;
    }
}
