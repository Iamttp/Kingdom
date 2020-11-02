using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 
/// TODO 多兵种拖拽
/// TODO 拖拽bug, 使用图标
/// TODO 进入最后一层 考虑
/// TODO 界面混乱 考虑
/// 
/// </summary>
public class Drag : MonoBehaviour
{
    public static Drag instance;

    [Header("初始属性")]
    public Vector3 startPos;
    public GameObject soldier;
    public string nameOfSoldier;

    [Header("来自Scene的属性")]
    public int height; // 8
    public int width; // 5
    public GameObject[,] boards;
    public GameObject[,] boardsUp;

    public Color ownerColor;
    public Color selectColor;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        guiMe = Resources.Load<GUISkin>("Textures/skin");
        style1 = guiMe.button;
        style2 = guiMe.label;

        height = Scene.instance.height;
        width = Scene.instance.width;
        boards = Scene.instance.boards;
        boardsUp = Scene.instance.boardsUp;

        ownerColor = Scene.instance.ownerColor;
        selectColor = Scene.instance.selectColor;

        startPos = transform.position;
    }

    void Update()
    {
    }

    private Vector3 screenPos;
    private Vector3 offset;

    void OnMouseDown()
    {
        screenPos = Camera.main.WorldToScreenPoint(transform.position);//获取物体的屏幕坐标     
        offset = screenPos - Input.mousePosition;//获取物体与鼠标在屏幕上的偏移量    
    }

    void OnMouseDrag()
    {
        //将拖拽后的物体屏幕坐标还原为世界坐标
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition + offset);
        upDateSel();
    }

    private GameObject nowBoard = null, lastBoard = null;
    void OnMouseUp()
    {
        // 释放鼠标 TODO OnMouseUp OnMouseUpAsButton
        if (nowBoard != null)
            if (nowBoard.GetComponent<Board>().isOwner && boardsUp[(int)nowBoard.transform.position.x, (int)nowBoard.transform.position.y] == null)
            {
                string soldierName = nameOfSoldier;
                soldier.GetComponent<Soldier>().soldierName = soldierName;
                // TODO
                if (SoldierManager.instance.dicSoldier[soldierName].needFood <= User.instance.foodVal &&
                        SoldierManager.instance.dicSoldier[soldierName].needMineral <= User.instance.mineralVal)
                {
                    User.instance.foodVal -= SoldierManager.instance.dicSoldier[soldierName].needFood;
                    User.instance.mineralVal -= SoldierManager.instance.dicSoldier[soldierName].needMineral;

                    GameObject now = Instantiate(soldier, nowBoard.transform.position + new Vector3(0, 0, -1), new Quaternion());
                    now.GetComponent<Soldier>().isOwner = true;
                    boardsUp[(int)nowBoard.transform.position.x, (int)nowBoard.transform.position.y] = now;
                    nowBoard.GetComponent<MeshRenderer>().material.color = ownerColor;
                }
            }

        if (lastBoard != null) lastBoard.GetComponent<MeshRenderer>().material.color = ownerColor;
        transform.position = startPos;
        nowBoard = lastBoard = null; // 一定复原
        screenPos = offset = new Vector3();
    }

    void upDateSel()
    {
        // 根据鼠标位置获取世界坐标，再通过世界坐标直接得到鼠标指向board
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 9)); // TODO
        int i = Mathf.RoundToInt(mouseWorldPos.x);
        int j = Mathf.RoundToInt(mouseWorldPos.y);
        if (i >= 0 && i < width && j >= 0 && j < 2) nowBoard = boards[i, j]; // 前两行
        else return;

        if (!nowBoard.GetComponent<Board>().isOwner) return;

        if (nowBoard == lastBoard) return;

        nowBoard.GetComponent<MeshRenderer>().material.color = selectColor;
        if (lastBoard != null) lastBoard.GetComponent<MeshRenderer>().material.color = ownerColor;
        lastBoard = nowBoard;
    }

    GUISkin guiMe;
    GUIStyle style1;
    GUIStyle style2;

    void OnGUI()
    {
        Vector2 mScreen = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 mPoint = new Vector2(mScreen.x, Screen.height - mScreen.y);
        GUI.Label(new Rect(mPoint.x - 100, mPoint.y + 10, 150, 70), nameOfSoldier, style2);
        GUI.Label(new Rect(mPoint.x - 200, mPoint.y + 80, 500, 70), SoldierManager.instance.dicSoldier[nameOfSoldier].needFood + " " + SoldierManager.instance.dicSoldier[nameOfSoldier].needMineral, style2);
    }
}
