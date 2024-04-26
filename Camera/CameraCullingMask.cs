using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCullingMask : MonoBehaviour
{
    [SerializeField] Camera selectedCamera;

    public void ShowLayerMask(string layerMask)
    {
        selectedCamera.cullingMask |= 1 << LayerMask.NameToLayer(layerMask);    
    }

    public void HideLayerMask(string layerMask)
    {
        selectedCamera.cullingMask &= ~(1 << LayerMask.NameToLayer(layerMask));
    }

    public void ToggleLayerMask(string layerMask)
    {
        selectedCamera.cullingMask ^= 1 << LayerMask.NameToLayer(layerMask);
    }
}
