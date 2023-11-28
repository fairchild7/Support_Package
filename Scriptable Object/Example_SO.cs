using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ExampleSO", menuName = "ScriptableObjects/ExampleSO", order = 1)]
public class ExampleSOData : ScriptableObject
{
    [SerializeField] ExampleObject[] objs;

    //[ContextMenu("RegenID")]
    //void RegenID()
    //{
    //    for (int i = 0; i < cars.Length; i++)
    //    {
    //        cars[i].id = i;
    //    }
    //}

    //[ContextMenu("AutoRegenStats")]
    //void RegenStats()
    //{
    //    for (int i = 0; i < cars.Length; i++)
    //    {
    //        cars[i].carStats.brake = Random.Range(5f, 9.5f);
    //        cars[i].carStats.drift = Random.Range(5f, 9.5f);
    //        cars[i].carStats.power = Random.Range(5f, 9.5f);
    //        cars[i].carStats.topSpeed = Random.Range(5f, 9.5f);
    //    }
    //}

    public ExampleObject GetObject(int id)
    {
        return objs[id];
    }

    public int Length => objs.Length;
}

[System.Serializable]
public class ExampleObject
{
    public int id;
}

//[System.Serializable]
//public class CarStats
//{
//    [Range(0f, 10f)]
//    public float topSpeed;
//    [Range(0f, 10f)]
//    public float power;
//    [Range(0f, 10f)]
//    public float brake;
//    [Range(0f, 10f)]
//    public float drift;
//}
