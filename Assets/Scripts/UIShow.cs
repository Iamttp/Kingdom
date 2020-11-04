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

    public RawImage red;

    public List<GameObject> dragList;

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
        foreach (var now in dragList)
        {
            string nameOfSoldier = now.GetComponent<Drag>().nameOfSoldier;
            var s = SoldierManager.instance.dicSoldier[nameOfSoldier];
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
        foreach (var now in dragList)
            DestroyImmediate(now);
        dragList.Clear();
    }

    public void goToSoldier()
    {
        PlaneOfMain.SetActive(false);
        PlaneOfSoldier.SetActive(true);
        foreach (var obj in dragObj)
        {
            GameObject now = Instantiate(obj);
            dragList.Add(now);
        }
    }


    public bool isTopCam;
    public GameObject topCam;
    public GameObject downCam;
    public Button soldierBtn;
    public void switchCam()
    {
        isTopCam = !isTopCam;
        topCam.SetActive(isTopCam);
        downCam.SetActive(!isTopCam);
        soldierBtn.enabled = isTopCam;
    }
}
