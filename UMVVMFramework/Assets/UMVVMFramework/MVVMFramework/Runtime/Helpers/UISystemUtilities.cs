using UnityEngine;

public static class UISystemUtilities
{
    /// <summary>
    /// Projection in local space is eligible for ONLY if the render mode of renderCanvas is "WorldSpace" and "ScreenSpaceCamera"
    /// </summary>
    /// <param name="targetRect"></param>
    /// <param name="worldPos"></param>
    /// <param name="targetCamera"></param>
    /// <param name="renderCanvas"></param>
    /// <param name="projectInLocalSpace"></param>
    /// <returns></returns>
    public static Vector2 WorldPosToCanvasPos(RectTransform targetRect, Vector3 worldPos, Camera targetCamera, Canvas renderCanvas, bool projectInLocalSpace)
    {
        switch (renderCanvas.renderMode)
        {
            case RenderMode.WorldSpace:
            case RenderMode.ScreenSpaceCamera:
                var resRatioBasedOnRef = targetCamera.pixelWidth / renderCanvas.GetComponent<RectTransform>().rect.width;
                return (worldPos / renderCanvas.scaleFactor) * resRatioBasedOnRef;
            case RenderMode.ScreenSpaceOverlay:
                RectTransform projectTransform = GetProjectionTransform(targetRect, renderCanvas, projectInLocalSpace);

                var projectAreaSize = new Vector2(projectTransform.rect.size.x * projectTransform.lossyScale.x, projectTransform.rect.size.y * projectTransform.lossyScale.y);
                var viewportPos = targetCamera.WorldToViewportPoint(worldPos);

                Vector2 padding = GetPaddingFromProjectionAreaToCanvas(projectTransform, projectAreaSize, renderCanvas);

                return new Vector2(projectAreaSize.x * viewportPos.x, projectAreaSize.y * viewportPos.y) + padding;
            default:
                return Vector2.zero;
        }
    }

    /// <summary>
    /// Projection in local space is eligible for ONLY if the render mode of renderCanvas is "WorldSpace" and "ScreenSpaceCamera"
    /// </summary>
    /// <param name="targetRect"></param>
    /// <param name="screenPos"></param>
    /// <param name="targetCamera"></param>
    /// <param name="renderCanvas"></param>
    /// <param name="projectInLocalSpace"></param>
    /// <returns></returns>
    public static Vector2 ScreenToCanvasPos(RectTransform targetRect, Vector2 screenPos, Camera targetCamera, Canvas renderCanvas, bool projectInLocalSpace)
    {
        switch(renderCanvas.renderMode)
        {
            case RenderMode.WorldSpace:
            case RenderMode.ScreenSpaceCamera:
                var resRatioBasedOnRef = targetCamera.pixelWidth / renderCanvas.GetComponent<RectTransform>().rect.width;
                return (targetCamera.ScreenToWorldPoint(screenPos) / renderCanvas.scaleFactor) * resRatioBasedOnRef;
            case RenderMode.ScreenSpaceOverlay:
                RectTransform projectTransform = GetProjectionTransform(targetRect, renderCanvas, projectInLocalSpace);

                var projectAreaSize = new Vector2(projectTransform.rect.size.x * projectTransform.lossyScale.x, projectTransform.rect.size.y * projectTransform.lossyScale.y);
                var viewportPos = targetCamera.ScreenToViewportPoint(screenPos);

                Vector2 padding = GetPaddingFromProjectionAreaToCanvas(projectTransform, projectAreaSize, renderCanvas);

                return new Vector2(projectAreaSize.x * viewportPos.x, projectAreaSize.y * viewportPos.y) + padding;
            default:
                return Vector2.zero;
        }
    }

    static Vector2 GetPaddingFromProjectionAreaToCanvas(RectTransform projectTransform, Vector2 projectAreaSize, Canvas renderCanvas)
    {
        var canvasTrans = ((RectTransform)renderCanvas.transform);
        var canvasSize = canvasTrans.rect.size * renderCanvas.scaleFactor;

        var widthDiff = canvasSize.x - projectAreaSize.x;
        var heightDiff = canvasSize.y - projectAreaSize.y;

        var posDiff = renderCanvas.transform.position - projectTransform.position;

        var leftPadding = widthDiff / 2f - posDiff.x;
        var bottomPadding = heightDiff / 2f - posDiff.y;

        return new Vector2(leftPadding, bottomPadding);
    }

    static RectTransform GetProjectionTransform(RectTransform targetRect, Canvas mainCanvas, bool projectInLocalSpace)
    {
        RectTransform projectTransform = (RectTransform)mainCanvas.transform;
        if (projectInLocalSpace)
            projectTransform = (RectTransform)targetRect.parent;

        return projectTransform;
    }
}