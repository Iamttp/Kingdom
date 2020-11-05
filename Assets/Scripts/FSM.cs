using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public string nameUnit;
    public UnitManager.node s;
    public bool isDeath;
    public bool isOwner;

    public static int height; // 8
    public static int width; // 5
    public static GameObject[,] boards;
    public static GameObject[,] boardsUp;

    public GUISkin guiMe;
    public GUIStyle style1;
    public GUIStyle style2;
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

// FSM
public abstract class StateObject
{
    public Soldier unit;
    protected StateObject(Soldier unit)
    {
        this.unit = unit;
    }

    public abstract void update(float deltaTime, ref StateObject state);
}

public class IdleObject : StateObject
{
    private float nowTime;

    public IdleObject(Soldier unit) : base(unit)
    {
        nowTime = 0;
    }

    bool isAttack(int index)
    {
        int i = (int)(unit.transform.position.x);
        if (index >= 0 && index < Unit.height)
            if (Unit.boardsUp[i, index] != null)
            {
                var tag = Unit.boardsUp[i, index].GetComponent<Unit>();
                if (tag.isOwner != unit.isOwner)
                {
                    return true;
                }
            }
        return false;
    }

    public override void update(float deltaTime, ref StateObject state)
    {
        if (unit.isDeath) return;
        unit.animator.SetBool("isChange", false);

        nowTime += deltaTime;
        if (nowTime >= unit.s.attackTime)
        {
            nowTime = 0;

            int index = (int)(unit.transform.position.y) + unit.s.attackDis * unit.order;
            if (isAttack(index))
            {
                state = new AttackState(unit);
            }
            else if (unit.s.attackDis == 2 && isAttack((int)(unit.transform.position.y) + unit.order)) // 考虑弓箭手情况
            {
                state = new AttackState(unit);
            }
            else
            {
                state = new GoState(unit);
            }
        }
    }
}

public class AttackState : StateObject
{
    public AttackState(Soldier unit) : base(unit)
    {
    }

    public override void update(float deltaTime, ref StateObject state) // TODO time
    {
        if (unit.isDeath) return;
        // ...
        unit.animator.SetTrigger("isAttack");
        int i = (int)(unit.transform.position.x);
        int index = (int)(unit.transform.position.y) + unit.s.attackDis * unit.order;
        if (index >= 0 && index < Unit.height)
        {
            if (Unit.boardsUp[i, index] == null && unit.s.attackDis == 2) // 考虑弓箭手情况
                index = (int)(unit.transform.position.y) + unit.order;
            if (Unit.boardsUp[i, index] != null)
            {
                // TODO
                Unit.boardsUp[i, index].GetComponent<Unit>().s.lifeVal -= Random.Range(unit.s.attackValMin, unit.s.attackValMax);
                if (Unit.boardsUp[i, index].GetComponent<Unit>().s.lifeVal <= 0)
                {
                    //DestroyImmediate(Soldier.boardsUp[i, index]); // TODO
                    Unit.boardsUp[i, index].GetComponent<Unit>().isDeath = true;
                    Unit.boardsUp[i, index] = null;
                }
            }
        }
        state = new IdleObject(unit);
    }
}

public class GoState : StateObject
{
    private float nowTime;

    Vector3 pos;

    public GoState(Soldier unit) : base(unit)
    {
        nowTime = 0;
        pos = unit.transform.position;
    }

    public override void update(float deltaTime, ref StateObject state)
    {
        if (unit.isDeath) return;

        unit.animator.SetBool("isChange", true);

        unit.transform.position = new Vector3(pos.x, pos.y + nowTime / unit.s.goTime * unit.order, pos.z);
        int j = (int)pos.y + unit.order;
        if (j >= 0 && j < Unit.height)
        {
            var obj = Unit.boardsUp[(int)pos.x, j];
            if (obj != null) // 紧急回退
            {
                unit.transform.position = pos;
                nowTime = 0;
                state = new IdleObject(unit);
                return;
            }
        }

        nowTime += deltaTime;
        if (nowTime >= unit.s.goTime)
        {
            nowTime = 0;

            int i = (int)(pos.x);
            int index = (int)(pos.y) + unit.order;
            if (index >= Unit.height || index < 0)
            {
                Unit.boardsUp[i, (int)(pos.y)] = null;
                if (unit.isOwner) Computer.instance.lifeVal -= unit.s.lifeVal;
                else User.instance.lifeVal -= unit.s.lifeVal;
                unit.isDeath = true;
                return;
            }

            if (index >= 0 && index < Unit.height)
            {
                var objTag = Unit.boardsUp[i, index];
                if (objTag != null && objTag.GetComponent<Unit>().isOwner == unit.isOwner) // 前面存在我方部队
                {
                    state = new IdleObject(unit);
                    return;
                }


                var objTag2 = Unit.boards[i, index];
                if (objTag2.GetComponent<Board>().isOwner != unit.isOwner &&
                    index < Unit.height - Computer.instance.allowRow && index >= Computer.instance.allowRow) // 最后两行不攻占
                {
                    objTag2.GetComponent<Board>().isOwner = unit.isOwner;
                    objTag2.GetComponent<Board>().UpdateColor();
                }

                Unit.boardsUp[i, (int)(pos.y)] = null;
                Unit.boardsUp[i, index] = unit.gameObject;
                unit.transform.position = new Vector3(unit.transform.position.x, index, unit.transform.position.z);
            }
            state = new IdleObject(unit);
        }
    }
}
