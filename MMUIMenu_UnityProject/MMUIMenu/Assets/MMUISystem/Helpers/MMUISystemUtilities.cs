using UnityEngine;

public static class MMUISystemUtilities
{
    public static Vector2 WorldPosToCanvasPos(RectTransform targetRect, Vector3 worldPos, Camera targetCamera, Canvas targetCanvas)
    {
        var canvasSizeDelta = ((RectTransform)targetCanvas.transform).sizeDelta;
        Vector2 canvasOffset = new Vector2(canvasSizeDelta.x * targetRect.anchorMin.x, canvasSizeDelta.y * targetRect.anchorMin.y);

        var viewportPos = targetCamera.WorldToViewportPoint(worldPos);
        return new Vector2(((RectTransform)targetCanvas.transform).sizeDelta.x * viewportPos.x, ((RectTransform)targetCanvas.transform).sizeDelta.y * viewportPos.y) - canvasOffset;
    }

    public static Vector3 CanvasPosToWorldPos(Vector2 canvasPos, Camera targetCamera, Canvas targetCanvas)
    {
        var viewportPos = new Vector2(canvasPos.x / ((RectTransform)targetCanvas.transform).sizeDelta.x, canvasPos.y / ((RectTransform)targetCanvas.transform).sizeDelta.y);

        return targetCamera.ViewportToWorldPoint(viewportPos);
    }
}