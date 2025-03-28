using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelectable : MonoBehaviour
{
    [SerializeField] protected GameObject selectedObject;
    [SerializeField] Button btn;
    [SerializeField] protected ButtonSelectableControl controller;
    [SerializeField] ExtendedButtonSelectableEffect extendedEffect;

    protected Action btnAction;
    protected bool isSelected = false;

#if UNITY_EDITOR
    [ContextMenu("Setup")]
    public void Setup()
    {
        selectedObject = transform.Find("Selected").gameObject;
        btn = GetComponent<Button>();
        extendedEffect = GetComponent<ExtendedButtonSelectableEffect>();
    }
#endif

    private void Awake()
    {
        OnAwake();    
    }

    protected virtual void OnAwake()
    {
        btn.onClick.AddListener(() => OnButtonClick(false));
    }

    public virtual void OnButtonClick(bool skipEffect)
    {
        if (!isSelected)
        {
            if (!skipEffect)
            {
                AudioManager.Ins.PlaySFX(SFXType.ClickButton);
            }

            btnAction?.Invoke();

            selectedObject.SetActive(true);
            isSelected = true;

            if (controller != null)
            {
                controller.OnButtonSelect(this, skipEffect);
                if (extendedEffect != null)
                {
                    extendedEffect.OnSelected(skipEffect);
                }
            }
        }
    }

    public void AddListener(Action action)
    {
        btnAction = action;
    }

    public void ResetState(bool skipEffect)
    {
        if (isSelected)
        {
            isSelected = false;
            selectedObject.SetActive(false);
            if (extendedEffect != null)
            {
                extendedEffect.OnDeselected(skipEffect);
            }
        }
    }
}
