using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour
{
    public static Computer instance;

    [Header("战斗属性")]
    public float timeOfGo;
    public List<string> SoldierNames;
    public int allowRow;

    [Header("AI属性")]
    public float aITime;
    public int foodVal;
    public int mineralVal;
    public int lifeVal;

    [Header("来自Scene的属性")]
    public int height; // 8
    public int width; // 5
    public GameObject[,] boards;
    public GameObject[,] boardsUp;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        height = Scene.instance.height;
        width = Scene.instance.width;
        boards = Scene.instance.boards;
        boardsUp = Scene.instance.boardsUp;

        //foodVal = mineralVal = 100; // 初始资源
        lifeVal = 1000;

        foreach (var item in SoldierManager.instance.dicSoldier)
            SoldierNames.Add(item.Key);
    }

    private float aITimeNow;

    void Update()
    {
        if (lifeVal < 0)
        {
            Debug.Log("WIN");
        }

        aITimeNow += Time.deltaTime;
        if (aITimeNow >= aITime)
        {
            aITimeNow = 0;

            //Debug.Log(foodVal + " " + mineralVal + " " + lifeVal);

            int tryTime = 3;
            while (tryTime-- > 0)
            {
                int i = Random.Range(0, width);
                int j = Random.Range(height - allowRow, height); // 后两行
                if (!boards[i, j].GetComponent<Board>().isOwner && boardsUp[i, j] == null)
                {
                    string soldierName = SoldierNames[Random.Range(0, SoldierNames.Count)];
                    var nodeNow = SoldierManager.instance.dicSoldier[soldierName];
                    // TODO
                    if (SoldierManager.instance.dicSoldier[soldierName].needFood <= foodVal &&
                    SoldierManager.instance.dicSoldier[soldierName].needMineral <= mineralVal)
                    {
                        foodVal -= nodeNow.needFood;
                        mineralVal -= nodeNow.needMineral;

                        Vector3 rotationVector = new Vector3(0, 0, 180);
                        Quaternion rotation = Quaternion.Euler(rotationVector);
                        nodeNow.prefab.GetComponent<Soldier>().soldierName = soldierName;

                        GameObject now = Instantiate(nodeNow.prefab, new Vector3(i, j, -1), rotation);
                        now.GetComponent<Soldier>().isOwner = false;
                        boardsUp[i, j] = now;
                        break;
                    }
                }
            }
        }
    }
}
