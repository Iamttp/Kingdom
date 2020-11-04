using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// FSM
public abstract class StateObject
{
    public Soldier soldier;
    protected StateObject(Soldier soldier)
    {
        this.soldier = soldier;
    }

    public abstract void update(float deltaTime, ref StateObject state);
}

public class IdleObject : StateObject
{
    public float idleTime = 1;
    private float nowTime;

    public IdleObject(Soldier soldier) : base(soldier)
    {
        nowTime = 0;
    }

    bool isAttack(int index)
    {
        int i = (int)(soldier.transform.position.x);
        if (index >= 0 && index < Soldier.height)
            if (Soldier.boardsUp[i, index] != null && Soldier.boardsUp[i, index].GetComponent<Soldier>().isOwner != soldier.isOwner)
            {
                return true;
            }
        return false;
    }

    public override void update(float deltaTime, ref StateObject state)
    {
        if (soldier.isDeath) return;
        soldier.animator.SetBool("isChange", false);

        nowTime += deltaTime;
        if (nowTime >= idleTime)
        {
            nowTime = 0;

            int index = (int)(soldier.transform.position.y) + soldier.s.attackDis * soldier.order;
            if (isAttack(index))
            {
                state = new AttackState(soldier);
            }
            else if (soldier.s.attackDis == 2 && isAttack((int)(soldier.transform.position.y) + soldier.order)) // 考虑弓箭手情况
            {
                state = new AttackState(soldier);
            }
            else
            {
                state = new GoState(soldier);
            }
        }
    }
}

public class AttackState : StateObject
{
    public AttackState(Soldier soldier) : base(soldier)
    {
    }

    public override void update(float deltaTime, ref StateObject state) // TODO time
    {
        if (soldier.isDeath) return;
        // ...
        soldier.animator.SetTrigger("isAttack");
        int i = (int)(soldier.transform.position.x);
        int index = (int)(soldier.transform.position.y) + soldier.s.attackDis * soldier.order;
        if (index >= 0 && index < Soldier.height)
        {
            if (Soldier.boardsUp[i, index] == null && soldier.s.attackDis == 2)
                index = (int)(soldier.transform.position.y) + soldier.order;
            Soldier.boardsUp[i, index].GetComponent<Soldier>().s.lifeVal -=
                Random.Range(soldier.s.attackValMin, soldier.s.attackValMax);
            if (Soldier.boardsUp[i, index].GetComponent<Soldier>().s.lifeVal <= 0)
            {
                //DestroyImmediate(Soldier.boardsUp[i, index]); // TODO
                Soldier.boardsUp[i, index].GetComponent<Soldier>().isDeath = true;
                Soldier.boardsUp[i, index] = null;
            }
            state = new IdleObject(soldier);
        }
    }
}

public class GoState : StateObject
{
    public float goTime = 1;
    private float nowTime;

    Vector3 pos;

    public GoState(Soldier soldier) : base(soldier)
    {
        nowTime = 0;
        pos = soldier.transform.position;
    }

    public override void update(float deltaTime, ref StateObject state)
    {
        if (soldier.isDeath) return;

        soldier.animator.SetBool("isChange", true);

        soldier.transform.position = new Vector3(pos.x, pos.y + nowTime * soldier.order, pos.z);
        int j = (int)pos.y + soldier.order;
        if (j >= 0 && j < Soldier.height)
        {
            var obj = Soldier.boardsUp[(int)pos.x, j];
            if (obj != null) // 紧急回退
            {
                soldier.transform.position = pos;
                nowTime = 0;
                state = new IdleObject(soldier);
                return;
            }
        }

        nowTime += deltaTime;
        if (nowTime >= goTime)
        {
            nowTime = 0;

            int i = (int)(pos.x);
            int index = (int)(pos.y) + soldier.order;
            if (index >= Soldier.height || index < 0)
            {
                Soldier.boardsUp[i, (int)(pos.y)] = null;
                if (soldier.isOwner) Computer.instance.lifeVal -= soldier.s.lifeVal;
                else User.instance.lifeVal -= soldier.s.lifeVal;
                soldier.isDeath = true;
                return;
            }

            if (index >= 0 && index < Soldier.height)
            {
                var objTag = Soldier.boardsUp[i, index];
                if (objTag != null && objTag.GetComponent<Soldier>().isOwner == soldier.isOwner)
                {
                    state = new IdleObject(soldier);
                    return;
                }


                var objTag2 = Soldier.boards[i, index];
                if (objTag2.GetComponent<Board>().isOwner != soldier.isOwner &&
                    index < Soldier.height - Computer.instance.allowRow && index >= Computer.instance.allowRow) // 最后两行不攻占
                {
                    objTag2.GetComponent<Board>().isOwner = soldier.isOwner;
                    objTag2.GetComponent<Board>().UpdateColor();
                }

                Soldier.boardsUp[i, (int)(pos.y)] = null;
                Soldier.boardsUp[i, index] = soldier.gameObject;
                soldier.transform.position = new Vector3(soldier.transform.position.x, index, soldier.transform.position.z);
                state = new IdleObject(soldier);
            }
        }
    }
}
