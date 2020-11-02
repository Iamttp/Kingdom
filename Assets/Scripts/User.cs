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
    public Text lifeText;

    public int foodVal;
    public int mineralVal;
    public int lifeVal;

    void Awake()
    {
        instance = this;
        foodVal = mineralVal = 100; // 初始资源
        lifeVal = 1000;
    }

    void Start()
    {
    }

    void Update()
    {
        if(lifeVal <= 0)
        {
            Debug.Log("GameOver");
        }
        foodText.text = "Food : " + foodVal;
        mineralText.text = "Mine : " + mineralVal;
        lifeText.text = "Life : " + lifeVal;
    }
}
