using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSelectableControl : MonoBehaviour
{
    [SerializeField] List<ButtonSelectable> btns;
    [SerializeField] bool haveDefaultSelected = false;
    [SerializeField] int defaultSelectedIndex = 0;

    private void Start()
    {
        if (haveDefaultSelected)
            btns[defaultSelectedIndex].OnButtonClick(true);    
    }

    public void OnButtonSelect(ButtonSelectable selectedButton, bool skipEffect)
    {
        foreach (var btn in btns)
        {
            if (btn != selectedButton)
            {
                btn.ResetState(skipEffect);
            }
        }
    }
}
