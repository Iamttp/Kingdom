using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class END : MonoBehaviour
{
    public Text text;

    void Start()
    {
        if (Global.instance.sceneFlag == 1)
            text.text = "YOU WIN !";
        if (Global.instance.sceneFlag == 2)
            text.text = "YOU LOST !";

    }

    void Update()
    {
        
    }

    public void backFun()
    {
        SceneManager.LoadScene(0);
    }
}
