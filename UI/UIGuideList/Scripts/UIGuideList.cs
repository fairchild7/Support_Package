using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGuideList : UICanvas
{
    [Header("Prefabs")]
    [SerializeField] Button guideButtonPrefab;

    [Header("UI Contents")]
    [SerializeField] Transform contentTf;
    [SerializeField] Image guideImage;
    [SerializeField] Text guideText;
    [SerializeField] Button btnClose;

    private Guide firstShowingGuide;

    private void Awake()
    {
        btnClose.onClick.AddListener(OnClose);
    }

    public override void Open()
    {
        base.Open();
        firstShowingGuide = null;
        InitContent();
    }

    public override void Close(float delayTime)
    {
        base.Close(delayTime);
    }

    public override void CloseDirectly()
    {
        base.CloseDirectly();
    }

    private void InitContent()
    {
        for (int i = 0; i < DataManager.Ins.guideData.Count; i++)
        {
            if (DataManager.Ins.IsUnlockedGuide(i))
            {
                Guide guide = DataManager.Ins.guideData.GetGuide(i);

                if (firstShowingGuide == null)
                {
                    firstShowingGuide = guide;
                    UpdateContent(firstShowingGuide);
                }

                Button b = Instantiate(guideButtonPrefab, contentTf);
                b.GetComponentInChildren<Text>().text = guide.summarizeText;
                b.onClick.AddListener(() => {
                    UpdateContent(guide);
                });
            }
        }
    }

    private void UpdateContent(Guide guide)
    {
        guideImage.sprite = guide.contentImage;
        guideText.text = guide.contentText;
    }

    private void OnClose()
    {
        Close(0f);
    }
}
