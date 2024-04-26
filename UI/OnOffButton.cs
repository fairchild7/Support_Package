using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnOffButton : MonoBehaviour
{
    [SerializeField] GameObject iconOn;
    [SerializeField] GameObject iconOff;
    [SerializeField] Button btn;

    private bool isOn;
    private ButtonClickAction onClickAction;

    public bool IsOn => isOn;

    private void Awake()
    {
        btn.onClick.AddListener(() =>
        {
            this.PostEvent(EventID.PlaySFX, SFXType.ButtonClick);
            SwitchState();
        });
    }

    public void SetOnClickAction(ButtonClickAction action)
    {
        onClickAction = action;
    }

    private void SwitchState()
    {
        SetState(!isOn);
    }

    public void SetState(bool isOn)
    {
        this.isOn = isOn;
        iconOn.SetActive(isOn);
        iconOff.SetActive(!isOn);

        onClickAction();
    }

    public void RefreshButton()
    {
        SetState(false);
    }
}
