using UnityEngine;

public static class MMUISystemUtilities
{
    public static Vector2 WorldPosToCanvasPos(RectTransform targetRect, Vector3 worldPos, Camera targetCamera, Canvas mainCanvas, bool projectInLocalSpace)
    {
        RectTransform projectTransform = GetProjectionTransform(targetRect, mainCanvas, projectInLocalSpace);

        Vector2 parentOffset = Vector2.zero;
        if (CheckIfProjectInWorldAndParentIsNotCanvas(targetRect, mainCanvas, projectInLocalSpace))
            parentOffset = GetOffsetCausedByParentAnchoring(targetRect, mainCanvas);

        var projectAreaSize = projectTransform.rect.size;
        Vector2 projectDistOffset = new Vector2(projectAreaSize.x * targetRect.anchorMin.x, projectAreaSize.y * targetRect.anchorMin.y) + parentOffset;

        var viewportPos = targetCamera.WorldToViewportPoint(worldPos);
        return new Vector2(projectAreaSize.x * viewportPos.x, projectAreaSize.y * viewportPos.y) - projectDistOffset;
    }

    public static Vector2 ScreenToCanvasPos(RectTransform targetRect, Vector2 screenPos, Camera targetCamera, Canvas mainCanvas, bool projectInLocalSpace)
    {
        RectTransform projectTransform = GetProjectionTransform(targetRect, mainCanvas, projectInLocalSpace);

        Vector2 parentOffset = Vector2.zero;
        if (CheckIfProjectInWorldAndParentIsNotCanvas(targetRect,mainCanvas, projectInLocalSpace))
            parentOffset = GetOffsetCausedByParentAnchoring(targetRect, mainCanvas);

        var projectAreaSize = projectTransform.rect.size;
        Vector2 projectDistOffset = new Vector2(projectAreaSize.x * targetRect.anchorMin.x, projectAreaSize.y * targetRect.anchorMin.y) + parentOffset;

        var viewportPos = targetCamera.ScreenToViewportPoint(screenPos);
        return new Vector2(projectAreaSize.x * viewportPos.x, projectAreaSize.y * viewportPos.y) - projectDistOffset;
    }

    static RectTransform GetProjectionTransform(RectTransform targetRect, Canvas mainCanvas, bool projectInLocalSpace)
    {
        RectTransform projectTransform = (RectTransform)mainCanvas.transform;
        if (projectInLocalSpace)
            projectTransform = (RectTransform)targetRect.parent;

        return projectTransform;
    }

    //Called when only projected to world space
    static Vector2 GetOffsetCausedByParentAnchoring(RectTransform targetRect, Canvas mainCanvas)
    {
        var parentRect = (RectTransform)targetRect.parent;
        var mainCanvasRect = (RectTransform)mainCanvas.transform;

        Vector2 sizeDeltaWithoutAnchor = mainCanvasRect.rect.size - parentRect.rect.size;

        Vector2 parentRectRightBottom = new Vector2(parentRect.position.x + (mainCanvas.scaleFactor * (parentRect.rect.size / 2f).x), parentRect.position.y - (mainCanvas.scaleFactor * (parentRect.rect.size / 2f).y));
        Vector2 mainCanvasRightBottomPos = new Vector2(mainCanvasRect.position.x + (mainCanvas.scaleFactor * (mainCanvasRect.rect.size / 2f).x), 0f);

        Vector2 offset = new Vector2(mainCanvasRightBottomPos.x - parentRectRightBottom.x, parentRectRightBottom.y);
        Vector2 offsetBasedOnCanvasScale = offset * (1 - mainCanvas.scaleFactor);

        offset.x = (sizeDeltaWithoutAnchor.x / 2f) - offset.x - offsetBasedOnCanvasScale.x;
        offset.y = -1 * ((sizeDeltaWithoutAnchor.y / 2f) - offset.y - offsetBasedOnCanvasScale.y);

        return offset;
    }

    static bool CheckIfProjectInWorldAndParentIsNotCanvas(RectTransform targetRect, Canvas mainCanvas, bool projectInLocalSpace)
    {
        return !projectInLocalSpace && (RectTransform)targetRect.parent != (RectTransform)mainCanvas.transform;
    }
}