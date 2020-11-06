using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [Header("块消息")]
    public float addTime;

    public bool isOwner;

    public int addType; // 0 food 1 mineral
    public int addVal;
    public Color[] colFood;
    public Color[] colMine;
    public int i, j;

    public void UpdateColor()
    {
        var mat = gameObject.GetComponent<MeshRenderer>().material;
        if (isOwner) mat.SetColor("_ColorOut", Scene.instance.ownerColor);
        else mat.SetColor("_ColorOut", Scene.instance.enemyColor);
    }

    GUISkin guiMe;
    GUIStyle style1 = new GUIStyle();
    GUIStyle style2 = new GUIStyle();
    void Start()
    {
        // 随机生成方式
        //addType = Random.Range(0, 2);
        //addVal = Random.Range(1, 3); // TODO

        // 固定board
        addType = Scene.boardsType[i, j] % 2;
        addVal = Scene.boardsType[i, j] / 2 + 1;

        guiMe = Resources.Load<GUISkin>("Textures/Board");
        style1 = guiMe.button;
        style2 = guiMe.label;

        var mat = gameObject.GetComponent<MeshRenderer>().material;
        if (addType == 0) mat.SetColor("_ColorIn", colFood[addVal - 1]);
        else mat.SetColor("_ColorIn", colMine[addVal - 1]);
        UpdateColor();
    }

    private float addTimeNow;
    void Update()
    {
        addTimeNow += Time.deltaTime;
        if (addTimeNow >= addTime)
        {
            addTimeNow = 0;
            if (addType == 0)
            {
                if (isOwner) User.instance.foodVal += addVal;
                else Computer.instance.foodVal += addVal;
            }
            else
            {
                if (isOwner) User.instance.mineralVal += addVal;
                else Computer.instance.mineralVal += addVal;
            }
        }
    }

    void OnGUI()
    {
        if (!UIShow.instance.isTopCam) return;

        // TODO
        //Vector2 mScreen = Camera.main.WorldToScreenPoint(transform.position);
        //Vector2 mPoint = new Vector2(mScreen.x, Screen.height - mScreen.y);
        //GUI.Label(new Rect(mPoint.x - 40, mPoint.y + 10, 150, 70), addVal.ToString(), style2);
    }
}
