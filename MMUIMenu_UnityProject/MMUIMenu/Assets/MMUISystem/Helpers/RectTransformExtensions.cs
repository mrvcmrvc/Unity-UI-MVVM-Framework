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
}
