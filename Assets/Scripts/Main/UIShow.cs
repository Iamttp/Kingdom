﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        goToSoldier(); // 默认士兵界面
    }

    int widthOfRed = 1080 / 2;
    int lastWidthOfRed;

    void Update()
    {
        if (!isTopCam)
        {
            if (Input.touchCount == 1) //单点触碰移动摄像机
            {
                if (Input.touches[0].phase == TouchPhase.Moved) //手指在屏幕上移动，移动摄像机
                {
                    downCam.transform.RotateAround(new Vector3(2, 3.5f, 0), new Vector3(0, 0, 1), -Input.touches[0].deltaPosition.x * Time.deltaTime * 2); // 绕 （2 3.5f 0） 旋转
                }
            }
        }

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

    public void goToSoldier()
    {
        foreach (var now in dragCastleList)
            DestroyImmediate(now);
        dragCastleList.Clear();

        if (dragSoldierList.Count == 0)
            foreach (var obj in dragSoldierObj)
            {
                GameObject now = Instantiate(obj);
                dragSoldierList.Add(now);
            }
    }

    public void goToCastle()
    {
        foreach (var now in dragSoldierList)
            DestroyImmediate(now);
        dragSoldierList.Clear();

        if (dragCastleList.Count == 0)
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

        if (isTopCam)
        {
            goToSoldier();
        }
        else
        {
            foreach (var now in dragCastleList)
                DestroyImmediate(now);
            dragCastleList.Clear();

            foreach (var now in dragSoldierList)
                DestroyImmediate(now);
            dragSoldierList.Clear();
        }
    }

    public void backFun()
    {
        SceneManager.LoadScene(0);
    }

    public Text dSpeed;
    public void timeDouble()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 2;
            dSpeed.text = "x2";
        }
        else if(Time.timeScale == 2)
        {
            Time.timeScale = 0;
            dSpeed.text = "x0";
        }
        else
        {
            Time.timeScale = 1;
            dSpeed.text = "x1";
        }
    }


    public GameObject PlaneOfInfo;
    public Text nameText;
    public Text typeText;
    public Text needFoodText;
    public Text needMineText;
    public Text attackText;
    public Text lifeText;
    public Text attackDisText;
}
