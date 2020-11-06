using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 
/// TODO 单位简介界面 长按进入 Panel 
/// TODO 攻击粒子特效，受击动画
/// TODO 分数统计
/// TODO 界面混乱
/// 
/// </summary>
public class Drag : MonoBehaviour
{
    public static Drag instance;

    [Header("初始属性")]
    public Vector3 startPos;
    public int type; // 0 soldier 1 castle
    public string nameOfUnit;

    [Header("来自Scene的属性")]
    public int height; // 8
    public int width; // 5
    public GameObject[,] boards;
    public GameObject[,] boardsUp;

    public Color ownerColor;
    public Color selectColor;


    [Header("来自Computer的属性")]
    public int allowRow;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        guiMe = Resources.Load<GUISkin>("Textures/Drag");
        style1 = guiMe.button;
        style2 = guiMe.label;

        height = Scene.height;
        width = Scene.width;
        boards = Scene.instance.boards;
        boardsUp = Scene.instance.boardsUp;

        ownerColor = Scene.instance.ownerColor;
        selectColor = Scene.instance.selectColor;

        startPos = transform.position;
        allowRow = Computer.instance.allowRow;
    }

    void Update()
    {
    }

    private Vector3 screenPos;
    private Vector3 offset;

    public float beginTime;
    public float endTime;
    public static bool isDrag;

    void OnMouseDown()
    {
        if (Time.timeScale == 0) return;

        isDrag = true; // TODO 拖动中
        screenPos = Camera.main.WorldToScreenPoint(transform.position);//获取物体的屏幕坐标     
        offset = screenPos - Input.mousePosition;//获取物体与鼠标在屏幕上的偏移量    

        beginTime = Time.time;

        for (int i = 0; i < width; i++)
            for (int j = 0; j < allowRow; j++)
            {
                nowBoard = boards[i, j];
                nowBoard.GetComponent<MeshRenderer>().material.SetColor("_ColorOut", Scene.instance.preSelectColor);
            }
    }

    void OnMouseDrag()
    {
        if (Time.timeScale == 0) return;

        //将拖拽后的物体屏幕坐标还原为世界坐标
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition + offset);
        upDateSel();
    }

    private GameObject nowBoard = null, lastBoard = null;

    void downUnit(int i, int j, UnitManager.node nodeNow)
    {
        // TODO
        if (nodeNow.needFood <= User.instance.foodVal && nodeNow.needMineral <= User.instance.mineralVal)
        {
            User.instance.foodVal -= nodeNow.needFood;
            User.instance.mineralVal -= nodeNow.needMineral;

            nodeNow.prefab.GetComponent<Unit>().nameUnit = nodeNow.name;
            GameObject now = Instantiate(nodeNow.prefab, new Vector3(i, j, -1), new Quaternion());
            now.GetComponent<Unit>().isOwner = true;
            boardsUp[i, j] = now;
            for (int ii = 0; ii < width; ii++)
                for (int jj = 0; jj < allowRow; jj++)
                {
                    nowBoard = boards[ii, jj];
                    nowBoard.GetComponent<MeshRenderer>().material.SetColor("_ColorOut", Scene.instance.ownerColor);
                }
        }
        //else
        //{
        //    // 资源不够
        //    if (SoldierManager.instance.dicSoldier[soldierName].needFood > User.instance.foodVal)
        //        StartCoroutine(User.instance.redText(User.instance.foodText, 3, User.instance.foodText.color));
        //    if (SoldierManager.instance.dicSoldier[soldierName].needMineral > User.instance.mineralVal)
        //        StartCoroutine(User.instance.redText(User.instance.mineralText, 3, User.instance.mineralText.color));
        //}
    }

    // 释放鼠标 TODO OnMouseUp OnMouseUpAsButton
    void OnMouseUp()
    {
        if (Time.timeScale == 0) return;

        isDrag = false; // TODO 未拖动
        endTime = Time.time;
        //Debug.Log(endTime - beginTime);
        if (endTime - beginTime < 0.1f) return; // 时间过短，判定为误触

        if (nowBoard == null) return;

        int i = (int)nowBoard.transform.position.x;
        int j = (int)nowBoard.transform.position.y;

        var nodeNow = UnitManager.instance.dicUnit[nameOfUnit];

        if (transform.position.x >= 0 - 0.5f && transform.position.x < width + 0.5f &&
            transform.position.y >= 0 - 0.5f && transform.position.y < allowRow + 0.5f)
        {
            if (boardsUp[i, j] == null)
            {
                downUnit(i, j, nodeNow);
            }
            else if (boardsUp[i, j].GetComponent<Unit>().s.type == 1) // 考虑城堡特殊情况 两城堡
            {
                while (boardsUp[i, j] != null && boardsUp[i, j].GetComponent<Unit>().s.type == 1)
                {
                    j++;
                }

                if ((j > allowRow) || (j == allowRow && nodeNow.type == 1) || boardsUp[i, j] != null && boardsUp[i, j].GetComponent<Unit>().s.type == 0)
                {
                    // j > allowRow 不放置
                    // j == allowRow 且为城堡 不放置
                    // 前方有部队，不放置
                }
                else
                {
                    downUnit(i, j, nodeNow);
                }
            }
        }

        nowBoard = lastBoard = null; // 一定复原
        screenPos = offset = new Vector3();
        transform.position = startPos;
    }

    void upDateSel()
    {
        // 根据鼠标位置获取世界坐标，再通过世界坐标直接得到鼠标指向board
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 9)); // TODO
        int i = Mathf.RoundToInt(mouseWorldPos.x);
        int j = Mathf.RoundToInt(mouseWorldPos.y);
        if (i >= 0 && i < width && j >= 0 && j < allowRow) nowBoard = boards[i, j]; // 前两行
        else return;

        if (!nowBoard.GetComponent<Board>().isOwner) return;

        if (nowBoard == lastBoard) return;

        nowBoard.GetComponent<MeshRenderer>().material.SetColor("_ColorOut", Scene.instance.selectColor);
        if (lastBoard != null) lastBoard.GetComponent<MeshRenderer>().material.SetColor("_ColorOut", Scene.instance.preSelectColor);
        lastBoard = nowBoard;
    }

    GUISkin guiMe;
    GUIStyle style1;
    GUIStyle style2;

    void OnGUI()
    {
        if (!UIShow.instance.isTopCam) return;

        Vector2 mScreen = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 mPoint = new Vector2(mScreen.x, Screen.height - mScreen.y);
        GUI.Label(new Rect(mPoint.x - 80, mPoint.y + 70, 150, 70), nameOfUnit, style2);
        //GUI.Label(new Rect(mPoint.x - 80, mPoint.y + 80, 250, 70), UnitManager.instance.dicUnit[nameOfUnit].needFood + " " + UnitManager.instance.dicUnit[nameOfUnit].needMineral, style2);
    }
}
