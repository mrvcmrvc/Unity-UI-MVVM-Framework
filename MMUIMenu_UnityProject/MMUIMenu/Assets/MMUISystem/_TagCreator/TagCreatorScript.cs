using MMUISystem.UIButton;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TagCreatorScript : MonoBehaviour
{
    public TMP_InputField InputField;
    public UnityUIButton AddUpdateButton, DeleteButton;

    public Action<string> OnTagAddPressed;
    public void FireOnTagAddPressed(string newTag)
    {
        if (OnTagAddPressed != null)
            OnTagAddPressed(newTag);
    }

    public Action<string> OnTagRemovePressed;
    public void FireOnTagRemovePressed(string newTag)
    {
        if (OnTagRemovePressed != null)
            OnTagRemovePressed(newTag);
    }

    private void Awake()
    {
        AddUpdateButton.OnButtonPressDown += OnAddPressed;
        DeleteButton.OnButtonPressDown += OnDeletePressed;

    }

    private void OnDestroy()
    {
        AddUpdateButton.OnButtonPressDown -= OnAddPressed;
        DeleteButton.OnButtonPressDown -= OnDeletePressed;
    }

    public void WriteTagToInputField(string tag)
    {
        InputField.text = tag;
    }

    private void OnAddPressed(PointerEventData eventData)
    {
        FireOnTagAddPressed(InputField.text.Trim());
    }

    private void OnDeletePressed(PointerEventData eventData)
    {
        FireOnTagRemovePressed(InputField.text.Trim());
    }
}
