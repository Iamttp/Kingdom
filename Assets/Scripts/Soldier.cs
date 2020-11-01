using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    [Header("士兵属性")]
    public int lifeVal;
    public int maxVal;
    public int attackVal;
    public int attackDis;
    public int order; // 1 -1 0 ， 1 我方前进 -1 敌方前进 0 进入攻击状态
    public bool isOwner;

    [Header("来自Computer的属性")]
    public float timeOfGo;

    [Header("来自Scene的属性")]
    public int height; // 8
    public int width; // 5
    public GameObject[,] boards;
    public GameObject[,] boardsUp;

    void Start()
    {
        guiMe = Resources.Load<GUISkin>("Textures/skin");
        
        timeOfGo = Computer.instance.timeOfGo;
        height = Scene.instance.height;
        width = Scene.instance.width;

        boards = Scene.instance.boards;
        boardsUp = Scene.instance.boardsUp;

        if (isOwner)
        {
            order = 1; // 士兵前进方向
            gameObject.GetComponent<MeshRenderer>().material.color = Scene.instance.ownerColor;
        }
        else
        {
            order = -1;
            gameObject.GetComponent<MeshRenderer>().material.color = Scene.instance.enemyColor;
        }

        lifeVal = maxVal = 10;
        attackVal = 3;
    }

    private float timeOfGoNow = 0;
    void Update()
    {
        timeOfGoNow += Time.deltaTime;
        if (timeOfGoNow >= timeOfGo)
        {
            timeOfGoNow = 0;

            int i = (int)transform.position.x;
            int j = (int)transform.position.y + order;
            if (j >= height || j < 0)
            {
                boardsUp[i, j - order] = null;
                DestroyImmediate(gameObject); // TODO
                return;
            }

            if (boardsUp[i, j] != null && boardsUp[i, j].GetComponent<Soldier>().isOwner != isOwner)
            {
                // Ready Attack
                boardsUp[i, j].GetComponent<Soldier>().lifeVal -= attackVal;
                if (boardsUp[i, j].GetComponent<Soldier>().lifeVal <= 0)
                {
                    DestroyImmediate(boardsUp[i, j]); // TODO
                    boardsUp[i, j] = null;
                    return;
                }
            }
            else if (boardsUp[i, j] != null && boardsUp[i, j].GetComponent<Soldier>().isOwner == isOwner) 
            {
                // ready wait
                return;
            }
            else
            {
                // Ready Move
                transform.position = new Vector3(i, j, transform.position.z);

                if (boards[i, j].GetComponent<Board>().isOwner != isOwner && j != height - 1 && j != 0) // 最后一行不攻占
                {
                    boards[i, j].GetComponent<Board>().isOwner = isOwner;
                    boards[i, j].GetComponent<Board>().UpdateColor();
                }
                boardsUp[i, j - order] = null;
                boardsUp[i, j] = gameObject;
            }
        }
    }

    GUISkin guiMe;
    GUIStyle style1 = new GUIStyle();
    GUIStyle style2 = new GUIStyle();

    private void OnGUI()
    {
        style1 = guiMe.button;
        style2 = guiMe.label;
        Vector2 mScreen = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 mPoint = new Vector2(mScreen.x, Screen.height - mScreen.y);
        GUI.Label(new Rect(mPoint.x - 40, mPoint.y + 10, 150, 70), lifeVal.ToString() + "/" + maxVal.ToString(), style2);
    }
}
