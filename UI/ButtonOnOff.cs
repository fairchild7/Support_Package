using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonOnOff : MonoBehaviour
{
    [SerializeField] GameObject iconOn;
    [SerializeField] GameObject iconOff;
    [SerializeField] Button btn;

    private bool isOn;
    private Action onClickAction;

    public bool IsOn => isOn;

#if UNITY_EDITOR
    [ContextMenu("Setup")]
    public void Setup()
    {
        iconOn = transform.Find("IconOn").gameObject;
        iconOff = transform.Find("IconOff").gameObject;
        btn = GetComponent<Button>();
    }
#endif

    private void Awake()
    {
        btn.onClick.AddListener(() =>
        {
            AudioManager.Ins.PlaySFX(SFXType.ClickButton);
            SwitchState();
        });
    }

    public void AddListener(Action action)
    {
        onClickAction = action;
    }

    private void SwitchState()
    {
        SetState(!isOn);
    }

    public void SetState(bool isOn, bool invokeAction = true)
    {
        this.isOn = isOn;
        iconOn.SetActive(isOn);
        iconOff.SetActive(!isOn);

        if (invokeAction)
            onClickAction?.Invoke();
    }

    public void RefreshButton()
    {
        SetState(false);
    }
}
