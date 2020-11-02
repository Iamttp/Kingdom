using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class User : MonoBehaviour
{
    public static User instance;

    [Header("玩家属性")]
    public Text foodText;
    public Text mineralText;

    public int foodVal;
    public int mineralVal;

    void Awake()
    {
        instance = this;
        foodVal = mineralVal = 100; // 初始资源
    }

    void Start()
    {
    }

    void Update()
    {
        foodText.text = "Food : " + foodVal;
        mineralText.text = "Mineral : " + mineralVal;
    }
}
