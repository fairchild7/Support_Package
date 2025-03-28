using UnityEngine;

public class ExtendedButtonSelectableEffect : MonoBehaviour
{
    private void Awake()
    {
        OnAwake();    
    }

    public virtual void OnAwake()
    {

    }

    public virtual void OnSelected(bool skipEffect = false)
    {

    }

    public virtual void OnDeselected(bool skipEffect = false)
    {

    }
}
