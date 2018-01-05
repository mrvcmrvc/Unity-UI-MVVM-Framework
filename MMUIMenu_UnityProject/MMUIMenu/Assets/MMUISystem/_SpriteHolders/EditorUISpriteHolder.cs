using System;
using UnityEngine;
using UnityEngine.U2D;

public class EditorUISpriteHolder : MonoBehaviour
{
    static EditorUISpriteHolder _instance;
    public static EditorUISpriteHolder Instance { get { return _instance; } }

    public SpriteAtlas EditorIconSpriteAtlas;

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

        Sprite targetSprite = EditorIconSpriteAtlas.GetSprite(targetEnum.ToString());

        if(targetSprite == null)
            Debug.LogError("<i><color=yellow>" + "Icon Sprite not found for: " + "</color><b><color=orange>" + sourceEnum + "</color></b></i>");

        return targetSprite;
    }
}
