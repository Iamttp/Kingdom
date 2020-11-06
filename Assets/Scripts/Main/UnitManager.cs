using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    // Inspector 不能看字典 用数组方法
    [System.Serializable]
    public struct node
    {
        public string name;
        public int type; // 0 soldier 1 castle

        public int lifeVal;
        public int maxVal;
        public int attackValMin;
        public int attackValMax;
        public int attackDis;
        public float attackTime;
        public float goTime; // only soldier

        public int needFood;
        public int needMineral;
        public GameObject prefab;
    }
    public node[] nodes;
    public Dictionary<string, node> dicUnit;

    public static UnitManager instance;

    void Awake()
    {
        instance = this;
        dicUnit = new Dictionary<string, node>();
        foreach (node x in nodes)
            dicUnit.Add(x.name, x);
    }
}
