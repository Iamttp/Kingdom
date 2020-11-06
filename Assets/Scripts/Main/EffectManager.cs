using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;
    public GameObject[] effectPrefabs;

    Queue<GameObject> effects; // 先出现的特效一定先消失 TODO 保证特效时间一样

    void Awake()
    {
        instance = this;
        effects = new Queue<GameObject>();
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void attackEffect(int index, Vector3 pos)
    {
        StartCoroutine(attackEffectI(index, pos));
    }

    void startEffect(int index, Vector3 pos)
    {
        effects.Enqueue(Instantiate(effectPrefabs[index], pos, new Quaternion()));
    }

    void stopEffect()
    {
        var now = effects.Dequeue();
        DestroyImmediate(now);
    }

    IEnumerator attackEffectI(int index, Vector3 pos)
    {
        startEffect(index, pos);
        yield return new WaitForSeconds(0.3f);
        stopEffect();
    }
}
