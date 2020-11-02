using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour
{
    public static Computer instance;

    [Header("战斗属性")]
    public float timeOfGo;
    public List<string> SoldierNames;

    [Header("AI属性")]
    public float aITime;
    public GameObject soldier;
    public int foodVal;
    public int mineralVal;

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

        foreach (var item in SoldierManager.instance.dicSoldier)
            SoldierNames.Add(item.Key);
    }

    private float aITimeNow;

    void Update()
    {
        aITimeNow += Time.deltaTime;
        if (aITimeNow >= aITime)
        {
            aITimeNow = 0;

            // TODO
            Debug.Log(foodVal + " " + mineralVal);

            int tryTime = 3;
            while (tryTime-- > 0)
            {
                int i = Random.Range(0, width);
                int j = Random.Range(height - 2, height); // 后两行
                if (!boards[i, j].GetComponent<Board>().isOwner && boardsUp[i, j] == null)
                {
                    string soldierName = SoldierNames[Random.Range(0, SoldierNames.Count)]; // TODO
                    soldier.GetComponent<Soldier>().soldierName = soldierName;
                    if (SoldierManager.instance.dicSoldier[soldierName].needFood <= foodVal &&
                        SoldierManager.instance.dicSoldier[soldierName].needMineral <= mineralVal)
                    {
                        foodVal -= SoldierManager.instance.dicSoldier[soldierName].needFood;
                        mineralVal -= SoldierManager.instance.dicSoldier[soldierName].needMineral;

                        Vector3 rotationVector = new Vector3(0, 0, 180);
                        Quaternion rotation = Quaternion.Euler(rotationVector);

                        GameObject now = Instantiate(soldier, new Vector3(i, j, -1), rotation);
                        now.GetComponent<Soldier>().isOwner = false;
                        boardsUp[i, j] = now;
                        break;
                    }
                }
            }
        }
    }
}
