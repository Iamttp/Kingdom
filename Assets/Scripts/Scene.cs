using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene : MonoBehaviour
{
    [Header("棋盘属性")]
    public int height; // 8
    public int width; // 5
    public GameObject board;

    public Color ownerColor;
    public Color enemyColor;
    public Color selectColor;

    [Header("棋盘变量")]
    public static Scene instance;
    public GameObject[,] boards;
    public GameObject[,] boardsUp;

    void Awake()
    {
        Application.targetFrameRate = 30;

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
            }
            for (int j = temp; j < height; j++)
            {
                boards[i, j] = Instantiate(board, new Vector3(i, j), new Quaternion(), transform);
                boards[i, j].GetComponent<Board>().isOwner = false;
            }
        }
    }
}
