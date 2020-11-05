using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene : MonoBehaviour
{
    [Header("棋盘属性")]
    public static int height = 8; // 8
    public static int width = 5; // 5

    /// <summary>
    /// addType = Scene.boardsType[i, j] % 2;
    /// addVal = Scene.boardsType[i, j] / 2 + 1;
    /// 
    ///     value   |   addType |   addVal
    ///     --------------------------------
    ///     0       |   0       |   1
    ///     1       |   1       |   1
    ///     2       |   0       |   2
    ///     3       |   1       |   2
    /// </summary>
    public static int[,] boardsType = new int[,]
    {
        // owner -----------> enemy
        { 2,0,1,3,3,1,0,2 },
        { 2,0,1,3,3,1,0,2 },
        { 2,0,1,3,3,1,0,2 },
        { 2,0,1,3,3,1,0,2 },
        { 2,0,1,3,3,1,0,2 },
    };
    public GameObject board;


    public Color ownerColor;
    public Color enemyColor;
    public Color selectColor;
    public Color preSelectColor;

    [Header("棋盘变量")]
    public static Scene instance;
    public GameObject[,] boards;
    public GameObject[,] boardsUp;

    void Awake()
    {
        Application.targetFrameRate = 30;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        instance = this;
        boards = new GameObject[width, height];
        boardsUp = new GameObject[width, height];
        initBoard();
    }

    void Start()
    {
    }

    void Update()
    {
    }

    void initBoard()
    {
        for (int i = 0; i < width; i++)
        {
            int temp = height / 2;
            for (int j = 0; j < temp; j++)
            {
                boards[i, j] = Instantiate(board, new Vector3(i, j), new Quaternion(), transform);
                boards[i, j].GetComponent<Board>().isOwner = true;
                boards[i, j].GetComponent<Board>().i = i;
                boards[i, j].GetComponent<Board>().j = j;
            }
            for (int j = temp; j < height; j++)
            {
                boards[i, j] = Instantiate(board, new Vector3(i, j), new Quaternion(), transform);
                boards[i, j].GetComponent<Board>().isOwner = false;
                boards[i, j].GetComponent<Board>().i = i;
                boards[i, j].GetComponent<Board>().j = j;
            }
        }
    }
}
