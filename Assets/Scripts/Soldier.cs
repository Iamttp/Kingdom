using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    [Header("来自Computer的属性")]
    public float timeOfGo;

    [Header("来自Scene的属性")]
    public int height; // 8
    public int width; // 5

    void Start()
    {
        timeOfGo = Computer.instance.timeOfGo;
        height = Scene.instance.height;
        width = Scene.instance.width;
    }

    private float timeOfGoNow = 0;
    void Update()
    {
        timeOfGoNow += Time.deltaTime;
        if (timeOfGoNow >= timeOfGo)
        {
            timeOfGoNow = 0;
            transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            if (transform.position.y >= height)
            {
                DestroyImmediate(gameObject); // TODO
                return;
            }
        }
    }
}
