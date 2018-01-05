using System;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using MMUISystem.UIButton;

public class TagUIScript : MonoBehaviour
{
    public UnityUIButton SelectButton;
    public GameObject SelectionObj;
    public TextMeshProUGUI TagText;

    private bool _isSelected;

    public string SegmentTag { get; private set; }

    public Action<string, bool> OnTagPressed;
    void FireOnTagPressed()
    {
        if (OnTagPressed != null)
            OnTagPressed(TagText.text, _isSelected);
    }

    private void Awake()
    {
        SelectButton.OnButtonPressDown += OnPressDown;

        SetContainerSelectionActive(false);
    }

    private void OnDestroy()
    {
        SelectButton.OnButtonPressDown -= OnPressDown;
    }

    public void SetContainerSelectionActive(bool isActive)
    {
        _isSelected = isActive;
        SelectionObj.SetActive(isActive);
    }

    private void OnPressDown(PointerEventData eventData)
    {
        SetContainerSelectionActive(!_isSelected);

        FireOnTagPressed();
    }

    public void SetTag(string tag)
    {
        SegmentTag = tag;

        TagText.text = SegmentTag;
    }
}
