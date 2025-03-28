using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScalingBackground : MonoBehaviour
{
    [Header("Default screen size you are working with")]
    [SerializeField] float defaultWidth = 1080;
    [SerializeField] float defaultHeight = 1920;

    [Header("Actual size of background image, need to have same scale with screen size. " +
        "\nIn this example, background image have size 1200 x 2400, " +
        "\nbut 1200 x 2133.33 will have same 9:16 ratio with 1080x1920" +
        "\nYou will need anchor center + middle to apply this script")]
    [SerializeField] float acceptableWidth = 1200;
    [SerializeField] float acceptableHeight = 2133.3333333f;

    private bool useAcceptableSize = false;

    void Start()
    {
        float currentScreenWidth = (float)Screen.width;
        float currentScreenHeight = (float)Screen.height;

        float widthRatio = currentScreenWidth / defaultWidth;
        float heightRatio = currentScreenHeight / defaultHeight;

        if (Mathf.Abs(widthRatio - heightRatio) > 0.1f)
        {
            useAcceptableSize = true;
        }

        if (widthRatio > heightRatio)
        {
            if (useAcceptableSize)
            {
                if (defaultWidth * widthRatio < acceptableWidth)
                {
                    return;
                }
                else
                {
                    float acceptableWidthRatio = currentScreenWidth / acceptableWidth;
                    transform.localScale *= acceptableWidthRatio;
                }
            }
            else
            {
                transform.localScale *= widthRatio;
            }
        }
        else
        {
            if (useAcceptableSize)
            {
                if (defaultHeight * heightRatio < acceptableHeight)
                {
                    return;
                }
                else
                {
                    float acceptableHeightRatio = currentScreenHeight / acceptableHeight;
                    transform.localScale *= acceptableHeightRatio;
                }
            }
            else
            {
                transform.localScale *= heightRatio;
            }
        }
    }
}
