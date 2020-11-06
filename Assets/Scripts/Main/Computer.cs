using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Computer : MonoBehaviour
{
    public static Computer instance;

    [Header("战斗属性")]
    public float timeOfGo;
    public List<string> UnitNames;
    public int allowRow;

    [Header("AI属性")]
    public float aITime;
    public int foodVal;
    public int mineralVal;
    public int lifeVal;
    public int addVal;

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
        height = Scene.height;
        width = Scene.width;
        boards = Scene.instance.boards;
        boardsUp = Scene.instance.boardsUp;

        //foodVal = mineralVal = 100; // 初始资源
        lifeVal = 1000;

        foreach (var item in UnitManager.instance.dicUnit)
            UnitNames.Add(item.Key);
    }

    private float aITimeNow;

    void Update()
    {
        if (lifeVal < 0)
        {
            Debug.Log("WIN");
            SceneManager.LoadScene(2);
        }

        aITimeNow += Time.deltaTime;
        if (aITimeNow >= aITime)
        {
            aITimeNow = 0;

            foodVal += addVal;      // AI 资源
            mineralVal += addVal;

            User.instance.foodVal += User.instance.addVal;
            User.instance.mineralVal += User.instance.addVal;

            //Debug.Log(foodVal + " " + mineralVal + " " + lifeVal);

            //int tryTime = 3;
            //while (tryTime-- > 0)
            //{
            //}

            int i = Random.Range(0, width);
            int j = Random.Range(height - allowRow, height); // 后两行
            if (boardsUp[i, j] == null) // 任意放置
            {
                foreach (var item in UnitManager.instance.dicUnit)
                {
                    var nodeNow = item.Value;
                    if (nodeNow.needFood <= foodVal && nodeNow.needMineral <= mineralVal)
                    {
                        if (Random.value > 0.5f) continue; // 防止一直是最小need的

                        foodVal -= nodeNow.needFood;
                        mineralVal -= nodeNow.needMineral;

                        Vector3 rotationVector = new Vector3(0, 0, 180);
                        Quaternion rotation = Quaternion.Euler(rotationVector);
                        nodeNow.prefab.GetComponent<Unit>().nameUnit = nodeNow.name;

                        GameObject now = Instantiate(nodeNow.prefab, new Vector3(i, j, -1), rotation);
                        now.GetComponent<Unit>().isOwner = false;
                        boardsUp[i, j] = now;
                        break;
                    }
                }
            }
            else if (boardsUp[i, j].GetComponent<Unit>().s.type == 1) // 城堡，放置单位
            {
                j--; // 前一格
                if (boardsUp[i, j] != null) return; // 前方存在部队
                foreach (var item in UnitManager.instance.dicUnit)
                {
                    var nodeNow = item.Value;
                    if (nodeNow.type == 1) continue;
                    if (nodeNow.needFood <= foodVal && nodeNow.needMineral <= mineralVal)
                    {
                        if (Random.value > 0.5f) continue; // 防止一直是最小need的

                        foodVal -= nodeNow.needFood;
                        mineralVal -= nodeNow.needMineral;

                        Vector3 rotationVector = new Vector3(0, 0, 180);
                        Quaternion rotation = Quaternion.Euler(rotationVector);
                        nodeNow.prefab.GetComponent<Unit>().nameUnit = nodeNow.name;

                        GameObject now = Instantiate(nodeNow.prefab, new Vector3(i, j, -1), rotation);
                        now.GetComponent<Unit>().isOwner = false;
                        boardsUp[i, j] = now;
                        break;
                    }
                }
            }
        }
    }
}
