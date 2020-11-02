using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    [Header("士兵属性")]
    public string soldierName;
    public Animator animator;
    SoldierManager.node s;

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
    }

    bool attackState(int i, int index)
    {
        if (index >= 0 && index < height)
            if (boardsUp[i, index] != null && boardsUp[i, index].GetComponent<Soldier>().isOwner != isOwner)
            {
                // Ready Attack
                animator.SetTrigger("isAttack");
                boardsUp[i, index].GetComponent<Soldier>().s.lifeVal -= s.attackVal;
                if (boardsUp[i, index].GetComponent<Soldier>().s.lifeVal <= 0)
                {
                    DestroyImmediate(boardsUp[i, index]); // TODO
                    boardsUp[i, index] = null;
                }
                return true;
            }
        return false;
    }

    private float timeOfGoNow = 0;
    private float timeOfAttackNow = 0;
    bool isAttack = false;

    void Update()
    {
        int i = (int)transform.position.x;
        int j = (int)transform.position.y + order;
        if (j >= height || j < 0)
        {
            boardsUp[i, j - order] = null;
            if (isOwner) Computer.instance.lifeVal -= s.lifeVal;
            else User.instance.lifeVal -= s.lifeVal;
            DestroyImmediate(gameObject); // TODO
            return;
        }

        timeOfAttackNow += Time.deltaTime;
        timeOfGoNow += Time.deltaTime;

        if (timeOfAttackNow >= s.attackTime)
        {
            timeOfAttackNow = 0;

            isAttack = false;
            int jAttack = j + s.attackDis * order;
            if (jAttack != j)
            {
                if (attackState(i, jAttack) || attackState(i, j)) // 先考虑远处，远处不中，再进攻近处
                {
                    isAttack = true;
                }
            }
            else
            {
                if (attackState(i, j))
                {
                    isAttack = true;
                }
            }
        }

        if (timeOfGoNow >= timeOfGo)
        {
            timeOfGoNow = 0;

            if (isAttack || (boardsUp[i, j] != null && boardsUp[i, j].GetComponent<Soldier>().isOwner == isOwner))
            {
                // Ready wait
                animator.SetBool("isChange", false);
            }
            else if (boardsUp[i, j] == null)
            {
                // Ready Move
                animator.SetBool("isChange", true);
                transform.position = new Vector3(i, j, transform.position.z);

                if (boards[i, j].GetComponent<Board>().isOwner != isOwner && j < height - Computer.instance.allowRow && j >= Computer.instance.allowRow) // 最后两行不攻占
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
        GUI.Label(new Rect(mPoint.x - 40, mPoint.y + 10, 150, 70), s.name, style2);
    }
}
