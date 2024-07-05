using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurvedText : MonoBehaviour
{
    [SerializeField] float radius = 100f;
    [SerializeField] Text textComponent;
    [SerializeField] float startAngle = -60f;
    [SerializeField] float endAngle = 60f;
    [SerializeField] float yOffset = -200f;

    void Start()
    {
        ApplyCurvedText();
    }

    public void ApplyCurvedText()
    {
        ClearCurvedText();

        string text = textComponent.text;
        int charCount = text.Length;

        float angleRange = endAngle - startAngle;
        float angleStep = angleRange / Mathf.Max(1, charCount - 1);

        GameObject parentObject = new GameObject("CurvedTextParent");
        parentObject.transform.SetParent(textComponent.transform);
        parentObject.transform.localPosition = Vector3.zero;
        parentObject.transform.localRotation = Quaternion.identity;

        for (int i = 0; i < charCount; i++)
        {
            GameObject charObj = new GameObject("Char" + i);
            charObj.transform.SetParent(parentObject.transform);
            charObj.transform.localPosition = Vector3.zero;
            charObj.transform.localRotation = Quaternion.identity;

            Text charText = charObj.AddComponent<Text>();
            charText.text = text[i].ToString();
            charText.font = textComponent.font;
            charText.fontSize = textComponent.fontSize;
            charText.color = textComponent.color;
            charText.alignment = TextAnchor.MiddleCenter;

            float charWidth = charText.preferredWidth;

            float angle = startAngle + angleStep * i;
            
            float x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
            float y = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;

            charObj.transform.localPosition = new Vector3(x, -y + yOffset, 0);
            charObj.transform.localRotation = Quaternion.Euler(0, 0, -(angle + 90));

            Outline outline = charObj.AddComponent<Outline>();
            outline.effectColor = new Vector4(0f, 0f, 1f, 1f);
            outline.effectDistance = new Vector2(2f, 2f);
        }

        textComponent.enabled = false;
    }

    public void ClearCurvedText()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        textComponent.enabled = true;
    }
}
