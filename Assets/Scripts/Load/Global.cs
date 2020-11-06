using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{
    public static Global instance;

    public int sceneFlag;

    // 游戏记录 TODO
    public float useTime;
    public int killCas;
    public int lostCas;
    public int killS;
    public int lostS;

    public void DataInit()
    {
        useTime = 0;
        killCas = 0;
        lostCas = 0;
        killS = 0;
        lostS = 0;
    }

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
            DataInit();
        }
    }

    void Update()
    {
    }

    public int getScore()
    {
        int scoreNum = instance.killCas * 100 + instance.killS * 10 -
    instance.lostCas * 10 - instance.lostS * 1;
        scoreNum += (1000 - (int)instance.useTime);
        if (scoreNum < 0) scoreNum = 0;
        return scoreNum;
    }
}
