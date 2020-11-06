using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;
    public GameObject[] effectPrefabs;

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

    public void attackEffect(int index, Vector3 pos, float time)
    {
        StartCoroutine(attackEffectI(index, pos, time));
    }

    IEnumerator attackEffectI(int index, Vector3 pos, float time)
    {
        var effect = Instantiate(effectPrefabs[index], pos, new Quaternion());
        yield return new WaitForSeconds(time);
        DestroyImmediate(effect);
    }
}
