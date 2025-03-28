using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using Unity.VisualScripting;
using UnityEngine;

public static class MyUtilities
{
    public static Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;

        return p;
    }

    public static T StringToTEnum<T>(string stringType) where T : struct, Enum
    {
        return (T)System.Enum.Parse(typeof(T), stringType);
    }

    public static Texture2D MakeTextureReadable(Texture2D originalTexture)
    {
        RenderTexture tempRenderTexture = RenderTexture.GetTemporary(
            originalTexture.width,
            originalTexture.height,
            0,
            RenderTextureFormat.Default,
            RenderTextureReadWrite.Linear);

        Graphics.Blit(originalTexture, tempRenderTexture);

        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = tempRenderTexture;

        Texture2D readableTexture = new Texture2D(originalTexture.width, originalTexture.height);
        readableTexture.ReadPixels(new Rect(0, 0, tempRenderTexture.width, tempRenderTexture.height), 0, 0);
        readableTexture.Apply();

        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(tempRenderTexture);

        return readableTexture;
    }

    /// <summary>
    /// Caches previous queries.
    /// </summary>
    private static readonly Dictionary<Animator, Dictionary<string, bool>> AnimatorParameterCache =
        new Dictionary<Animator, Dictionary<string, bool>>();

    /// <summary>
    /// Returns true if an animator has a parameter.
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="paramName"></param>
    /// <returns></returns>
    public static bool HasParameter(this Animator animator, string paramName)
    {
        //Check, cache and return
        if (AnimatorParameterCache.ContainsKey(animator) && AnimatorParameterCache[animator].ContainsKey(paramName))
            return AnimatorParameterCache[animator][paramName];

        //Create the dictionary if we need to
        if (!AnimatorParameterCache.ContainsKey(animator) || AnimatorParameterCache[animator] == null)
        {
            AnimatorParameterCache[animator] = new Dictionary<string, bool>();
        }

        AnimatorParameterCache[animator][paramName] =
            animator.parameters.Any(param => param.name == paramName);

        return AnimatorParameterCache[animator][paramName];
    }

    public static Transform GetNearestTransformFromList(Transform owner, List<Transform> targets)
    {
        float minDist = -1;
        Transform minTf = null;
        foreach (var obj in targets)
        {
            float dist = Vector3.Distance(obj.position, owner.position);
            if (minDist == -1)
            {
                minTf = obj;
                minDist = dist;
            }
            else
            {
                if (minDist > dist)
                {
                    minTf = obj;
                    minDist = dist;
                }
            }
        }

        return minTf;
    }

    public static void ChangeAllLayer(string value, GameObject go)
    {
        go.layer = LayerMask.NameToLayer(value);

        foreach (Transform child in go.transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer(value);
        }
    }

    public static string BeautifyText(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        StringBuilder beautifiedText = new StringBuilder();
        beautifiedText.Append(input[0]);

        for (int i = 1; i < input.Length; i++)
        {
            char currentChar = input[i];
            if (char.IsUpper(currentChar))
            {
                beautifiedText.Append(' ');
            }
            beautifiedText.Append(currentChar);
        }

        return beautifiedText.ToString();
    }

    public static Vector3 GetScreenPosition(Transform tf)
    {
        Vector3 worldPosition = tf.position;

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

        return screenPosition;
    }

    public static string FormatDateTime(DateTime dt)
    {
        return dt.ToString("HH:mm dd/MM/yyyy");
    }

    public static string MinuteSecondTimeFormat(float timer)
    {
        int minute = Mathf.FloorToInt(timer / 60f);
        int second = Mathf.FloorToInt(timer % 60f);
        return $"{minute:00}:{second:00}";
    }
}
