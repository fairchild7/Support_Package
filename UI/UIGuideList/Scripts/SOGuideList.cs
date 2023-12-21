using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "GuideList", menuName = "ScriptableObjects/GuideList", order = 1)]
public class SOGuideList : ScriptableObject
{
    [SerializeField] Guide[] guideList;

    public Guide GetGuide(int id)
    {
        return guideList[id];
    }

    public int Count => guideList.Length;
}

[System.Serializable]
public class Guide
{
    public int id;
    public string summarizeText;
    public Sprite contentImage;
    [Multiline] public string contentText;
}