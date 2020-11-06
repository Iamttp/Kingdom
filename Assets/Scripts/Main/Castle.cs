using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : Unit
{
    public GameObject cube;
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
    }

    private float timeOfAttackNow;

    void Update()
    {
        if (isDeath)
        {
            DestroyImmediate(gameObject);
            return;
        }
        // 判断周围存在soldier 进行攻击
        timeOfAttackNow += Time.deltaTime;
        if (timeOfAttackNow > s.attackTime)
        {
            timeOfAttackNow = 0;
            for (int i = -s.attackDis; i <= s.attackDis; i++)
                for (int j = -s.attackDis; j <= s.attackDis; j++)
                {
                    if (i == 0 && j == 0) continue;
                    int newX = (int)transform.position.x + i;
                    int newY = (int)transform.position.y + j;
                    if (newX >= 0 && newX < width && newY >= 0 && newY < height)
                    {
                        var tag = boardsUp[newX, newY];
                        if (tag != null && tag.GetComponent<Unit>().isOwner != isOwner)
                        {
                            tag.GetComponent<Soldier>().s.lifeVal -= Random.Range(s.attackValMin, s.attackValMax);
                            if (tag.GetComponent<Soldier>().s.lifeVal <= 0)
                            {
                                //DestroyImmediate(Soldier.boardsUp[i, index]); // TODO
                                tag.GetComponent<Soldier>().isDeath = true;
                                boardsUp[newX, newY] = null;
                            }
                            EffectManager.instance.attackEffect(0, new Vector3(newX, newY, transform.position.z));
                            return; // return 攻击一次 TODO
                        }
                    }
                }
        }
    }
}
