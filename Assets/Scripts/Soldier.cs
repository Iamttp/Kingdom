using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Unit
{
    [Header("士兵属性")]
    public Animator animator;
    public int order; // 1 -1 0 ， 1 我方前进 -1 敌方前进 0 进入攻击状态

    [Header("来自Computer的属性")]
    public float timeOfGo;

    void Start()
    {
        guiMe = Resources.Load<GUISkin>("Textures/Soldier");
        style1 = guiMe.button;
        style2 = guiMe.label;

        height = Scene.height;
        width = Scene.width;

        boards = Scene.instance.boards;
        boardsUp = Scene.instance.boardsUp;

        s = UnitManager.instance.dicUnit[nameUnit];

        timeOfGo = Computer.instance.timeOfGo;
        if (isOwner)
        {
            order = 1; // 士兵前进方向
        }
        else
        {
            order = -1;
        }
        animator.SetBool("isChange", true);
        currentState = new IdleObject(this);

        int i = (int)transform.position.x;
        int index = (int)transform.position.y;
        var objTag2 = Unit.boards[i, index];
        if (objTag2.GetComponent<Board>().isOwner != isOwner &&
            index < Unit.height - Computer.instance.allowRow && index >= Computer.instance.allowRow) // 最后两行不攻占
        {
            objTag2.GetComponent<Board>().isOwner = isOwner;
            objTag2.GetComponent<Board>().UpdateColor();
        }
    }

    private StateObject currentState;

    void Update()
    {
        if (isDeath)
        {
            DestroyImmediate(gameObject);
            return;
        }
        currentState.update(Time.deltaTime, ref currentState);
    }
}
