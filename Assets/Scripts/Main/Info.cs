using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Info : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        if (!UIShow.instance.isTopCam || Drag.isDrag) return;
        if (Input.GetMouseButton(0))
        {
            // STOP false;
            unshowInfo();

            // 根据鼠标位置获取世界坐标，再通过世界坐标直接得到鼠标指向board
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 9)); // TODO
            int i = Mathf.RoundToInt(mouseWorldPos.x);
            int j = Mathf.RoundToInt(mouseWorldPos.y);
            if (i < 0 || i >= Scene.width || j < 0 || j >= Scene.height) return;
            var now = Scene.instance.boardsUp[i, j];
            if (now == null) return;

            // STOP true;
            showInfo(now.GetComponent<Unit>().s, now.GetComponent<Unit>().isOwner);
        }
    }

    float lastScale = 1;
    void unshowInfo()
    {
        if (Time.timeScale.Equals(0))
        {
            Time.timeScale = lastScale;
            if ((int)lastScale == 1) UIShow.instance.dSpeed.text = "x1";
            else if ((int)lastScale == 2) UIShow.instance.dSpeed.text = "x2";
        }
        UIShow.instance.PlaneOfInfo.SetActive(false);
    }

    void showInfo(UnitManager.node s, bool isOwner)
    {
        if (!Time.timeScale.Equals(0))
        {
            lastScale = Time.timeScale;
            Time.timeScale = 0;
            UIShow.instance.dSpeed.text = "x0";
        }
        UIShow.instance.PlaneOfInfo.SetActive(true);
        UIShow.instance.nameText.text = "Name : " + s.name;
        UIShow.instance.typeText.text = isOwner ? "OUR" : "ENEMY";
        UIShow.instance.needFoodText.text = "Need Food : " + s.needFood;
        UIShow.instance.needMineText.text = "Nedd Mine : " + s.needMineral;
        UIShow.instance.attackText.text = "Attack Val : [" + s.attackValMin + " , " + s.attackValMax + "]";
        UIShow.instance.lifeText.text = "Life Val : " + s.lifeVal + " / " + s.maxVal;

        if (s.type == 1) UIShow.instance.attackDisText.text = "Attack Dis : " + "Eight Dir"; // TODO
        else UIShow.instance.attackDisText.text = "Attack Dis : " + s.attackDis;
    }
}