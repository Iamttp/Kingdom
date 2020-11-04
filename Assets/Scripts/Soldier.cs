using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    [Header("士兵属性")]
    public string soldierName;
    public Animator animator;
    public SoldierManager.node s;

    public int order; // 1 -1 0 ， 1 我方前进 -1 敌方前进 0 进入攻击状态
    public bool isOwner;
    public bool isDeath;

    [Header("来自Computer的属性")]
    public float timeOfGo;

    [Header("来自Scene的属性")]
    public static int height; // 8
    public static int width; // 5
    public static GameObject[,] boards;
    public static GameObject[,] boardsUp;

    void Start()
    {
        guiMe = Resources.Load<GUISkin>("Textures/Soldier");
        style1 = guiMe.button;
        style2 = guiMe.label;

        timeOfGo = Computer.instance.timeOfGo;
        height = Scene.instance.height;
        width = Scene.instance.width;

        boards = Scene.instance.boards;
        boardsUp = Scene.instance.boardsUp;

        if (isOwner)
        {
            order = 1; // 士兵前进方向
        }
        else
        {
            order = -1;
        }

        s = SoldierManager.instance.dicSoldier[soldierName];
        animator.SetBool("isChange", true);
        currentState = new IdleObject(this);
    }

    private StateObject currentState;

    void Update()
    {
        if (isDeath) DestroyImmediate(gameObject);
        currentState.update(Time.deltaTime, ref currentState);
    }

    GUISkin guiMe;
    GUIStyle style1;
    GUIStyle style2;
    //红色血条贴图
    public Texture2D blood_red;
    public Texture2D blood_blue;
    //黑色血条贴图
    public Texture2D blood_black;

    void OnGUI()
    {
        if (!UIShow.instance.isTopCam) return;

        Vector2 mScreen = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 mPoint = new Vector2(mScreen.x, Screen.height - mScreen.y);

        Vector2 bloodSize = GUI.skin.label.CalcSize(new GUIContent(blood_red));

        //通过血值计算红色血条显示区域
        int blood_width = blood_red.width * s.lifeVal / s.maxVal;
        //先绘制黑色血条
        GUI.DrawTexture(new Rect(mPoint.x - (bloodSize.x / 2), mPoint.y - bloodSize.y, bloodSize.x, bloodSize.y), blood_black);
        //在绘制红色血条
        if (isOwner) GUI.DrawTexture(new Rect(mPoint.x - (bloodSize.x / 2), mPoint.y - bloodSize.y, blood_width, bloodSize.y), blood_red);
        else GUI.DrawTexture(new Rect(mPoint.x - (bloodSize.x / 2), mPoint.y - bloodSize.y, blood_width, bloodSize.y), blood_blue);
        // name todo
        //GUI.Label(new Rect(mPoint.x - 40, mPoint.y + 10, 150, 70), s.name, style2);
    }
}
