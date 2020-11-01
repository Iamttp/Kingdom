﻿using System.Collections;
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
            while (true)
            {
                int i = Random.Range(0, width);
                int j = Random.Range(height - 2, height); // 后两行
                if (!boards[i, j].GetComponent<Board>().isOwner && boardsUp[i, j] == null)
                {
                    soldier.GetComponent<Soldier>().soldierName = SoldierNames[Random.Range(0, SoldierNames.Count)]; // TODO
                    GameObject now = Instantiate(soldier, new Vector3(i, j, -1), new Quaternion());
                    now.GetComponent<Soldier>().isOwner = false;
                    boardsUp[i, j] = now;
                    break;
                }
            }
        }
    }
}
