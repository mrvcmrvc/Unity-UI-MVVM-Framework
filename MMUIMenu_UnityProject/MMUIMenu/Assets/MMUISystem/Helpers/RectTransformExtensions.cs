using UnityEngine;

public static class RectTransformExtensions
{
    public static void SetPivotWithCounterAdjustPosition(this RectTransform rectTrans, Vector2 newPivot)
    {
        Vector2 pivotDelta = newPivot - rectTrans.pivot;
        Vector2 posDelta = new Vector2(rectTrans.sizeDelta.x * pivotDelta.x, rectTrans.sizeDelta.y * pivotDelta.y);

        rectTrans.pivot = newPivot;
        rectTrans.anchoredPosition += posDelta;
    }

    /// <summary>
    /// Use this method if target rectTransform's size changed
    /// </summary>
    /// <param name="rectTrans"></param>
    /// <param name="newPivot"></param>
    /// <param name="initialSize"></param>
    public static void SetPivotWithCounterAdjustPosition(this RectTransform rectTrans, Vector2 newPivot, Vector2 initialSize)
    {
        Vector2 pivotDelta = newPivot - rectTrans.pivot;
        Vector2 posDelta = new Vector2(initialSize.x * pivotDelta.x, initialSize.y * pivotDelta.y);

        rectTrans.pivot = newPivot;
        rectTrans.anchoredPosition += posDelta;
    }

    public static bool RectOverlaps(this RectTransform rectTrans1, RectTransform rectTrans2)
    {
        Rect rect1 = new Rect(rectTrans1.position.x + rectTrans1.rect.min.x, rectTrans1.position.y + rectTrans1.rect.min.y, rectTrans1.rect.width, rectTrans1.rect.height);
        Rect rect2 = new Rect(rectTrans2.position.x + rectTrans2.rect.min.x, rectTrans2.position.y + rectTrans2.rect.min.y, rectTrans2.rect.width, rectTrans2.rect.height);

        return rect1.Overlaps(rect2);
    }
}
