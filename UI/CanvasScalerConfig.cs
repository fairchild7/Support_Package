using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * add to parent canvas
 * */
public class CanvasScalerConfig : MonoBehaviour
{
    private void Awake()
    {
        CanvasScaler scaler = GetComponent<CanvasScaler>();
        if ((float)Screen.height / Screen.width >= 1920f / 1080f)
        {
            scaler.matchWidthOrHeight = 0f;
        }
        else
        {
            scaler.matchWidthOrHeight = 1f;
        }
    }
}
