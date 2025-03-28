using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class IconMoveUpOnSelected : ExtendedButtonSelectableEffect
{
    [SerializeField] Image icon;
    [SerializeField] float defaultIconScale = 0.7f;
    [SerializeField] float selectedIconScale = 1;

    private float selectedY;
    private float deselectedY;

    public override void OnAwake()
    {
        selectedY = icon.rectTransform.anchoredPosition.y + 80;
        deselectedY = icon.rectTransform.anchoredPosition.y;
    }

    public override void OnSelected(bool skipEffect = false)
    {
        base.OnSelected();

        icon.rectTransform.DOAnchorPosY(selectedY, skipEffect ? 0 : 0.2f);
        icon.transform.DOScale(selectedIconScale, skipEffect ? 0 : 0.2f);
    }

    public override void OnDeselected(bool skipEffect = false)
    {
        base.OnDeselected();
        icon.rectTransform.DOAnchorPosY(deselectedY, skipEffect ? 0 : 0.1f);
        icon.transform.DOScale(defaultIconScale, skipEffect ? 0 : 0.1f);
    }
}
