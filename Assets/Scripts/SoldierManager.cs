using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierManager : MonoBehaviour
{
    // Inspector 不能看字典 用数组方法
    [System.Serializable]
    public struct node
    {
        public string name;

        public int lifeVal;
        public int maxVal;
        public int attackVal;
        public int attackDis;
        public float attackTime;

        public int needFood;
        public int needMineral;
        //public GameObject prefab; // TODO
        public Color col; // TODO
    }
    public node[] nodes;
    public Dictionary<string, node> dicSoldier;

    public static SoldierManager instance;

    void Awake()
    {
        instance = this;
        dicSoldier = new Dictionary<string, node>();
        foreach (node x in nodes)
            dicSoldier.Add(x.name, x);
    }
}
