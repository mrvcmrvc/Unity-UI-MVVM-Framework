using System;
using UnityEngine;
using UnityEngine.U2D;

public class UIItemSpriteHolder : MonoBehaviour
{
    static UIItemSpriteHolder _instance;
    public static UIItemSpriteHolder Instance { get { return _instance; } }

    public SpriteAtlas IconSpriteAtlas, UIItemPreviewAtlas;

    private void Awake()
    {
        _instance = this;
    }

    private void OnDestroy()
    {
        _instance = null;
    }

    public Sprite GetIconSprite(IConvertible sourceEnum)
    {
        IconEnum targetEnum = Utilities.GetIconEnum(sourceEnum);

        Sprite targetSprite = IconSpriteAtlas.GetSprite(targetEnum.ToString());

        return targetSprite;
    }

    public Sprite GetUIItemPreviewSprite(IConvertible sourceEnum)
    {
        UIItemPreviewEnum targetEnum = Utilities.GetUIWearableEnum(sourceEnum);

        Sprite targetSprite = UIItemPreviewAtlas.GetSprite(targetEnum.ToString());

        return targetSprite;
    }
}
