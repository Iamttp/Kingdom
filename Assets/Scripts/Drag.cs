using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    [Header("初始属性")]
    public Vector3 startPos;
    public GameObject soldier;
    public List<GameObject> childs;

    [Header("来自Scene的属性")]
    public int height; // 8
    public int width; // 5
    public GameObject[,] boards;

    public Color defColor;
    public Color selColor;

    void Start()
    {
        height = Scene.instance.height;
        width = Scene.instance.width;
        boards = Scene.instance.boards;

        defColor = Scene.instance.defColor;
        selColor = Scene.instance.selColor;

        childs = new List<GameObject>();
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
        GameObject now = Instantiate(soldier, nowBoard.transform.position + new Vector3(0, 0, -1), new Quaternion());
        childs.Add(now);
        nowBoard.GetComponent<MeshRenderer>().material.color = defColor;
        transform.position = startPos;
        nowBoard = lastBoard = null; // 一定复原
    }

    void upDateSel()
    {
        // 根据鼠标位置获取世界坐标，再通过世界坐标直接得到鼠标指向board
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 9)); // TODO
        int i = Mathf.RoundToInt(mouseWorldPos.x);
        int j = Mathf.RoundToInt(mouseWorldPos.y);
        if (i >= 0 && i < width && j >= 0 && j < height) nowBoard = boards[i, j];
        if (nowBoard == lastBoard) return;
        nowBoard.GetComponent<MeshRenderer>().material.color = selColor;
        if (lastBoard != null) lastBoard.GetComponent<MeshRenderer>().material.color = defColor;
        lastBoard = nowBoard;
    }
}
