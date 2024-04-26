using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public float Vertical => touchInput.y;
    public float Horizontal => touchInput.x;

    private Vector2 touchInput, prevDelta, dragInput;

    private void Update()
    {
        touchInput = (dragInput - prevDelta) / Time.deltaTime;
        prevDelta = dragInput;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        prevDelta = dragInput = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        touchInput = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragInput = eventData.position;
    }
}
