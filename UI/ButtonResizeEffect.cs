using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonResizeEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] RectTransform rectTransform;
    private Vector3 originalScale;
    [SerializeField] float shrinkScale = 0.8f;

    Tween scaleTween;

    void Start()
    {
        originalScale = rectTransform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        scaleTween = rectTransform.DOScale(originalScale * shrinkScale, 0.25f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        scaleTween.Kill();
        scaleTween = rectTransform.DOScale(originalScale, 0.25f);
    }
}
