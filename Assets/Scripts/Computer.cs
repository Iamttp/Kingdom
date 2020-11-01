using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour
{
    public static Computer instance;

    [Header("战斗属性")]
    public float timeOfGo;

    void Awake()
    {
        instance = this;    
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
