using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [Header("块消息")]
    public bool isOwner;

    public void UpdateColor()
    {
        if (isOwner) gameObject.GetComponent<MeshRenderer>().material.color = Scene.instance.ownerColor;
        else gameObject.GetComponent<MeshRenderer>().material.color = Scene.instance.enemyColor;
    }

    void Start()
    {
        UpdateColor();
    }

    void Update()
    {
        
    }
}
