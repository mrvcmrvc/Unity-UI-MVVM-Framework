using UnityEngine;

public static class MMUISystemUtilities
{
    public static Vector2 WorldPosToCanvasPos(RectTransform targetRect, Vector3 worldPos, Camera targetCamera, Canvas targetCanvas, bool projectInLocalSpace)
    {
        RectTransform projectTransform = GetProjectionTransform(targetRect, targetCanvas, projectInLocalSpace);

        Vector2 parentOffset = Vector2.zero;
        if (CheckIfProjectInWorldAndParentIsNotCanvas(targetRect, targetCanvas, projectInLocalSpace))
            parentOffset = GetOffsetCausedByParentAnchoring(targetRect);

        var projectAreaSize = projectTransform.rect.size;
        Vector2 projectDistOffset = new Vector2(projectAreaSize.x * targetRect.anchorMin.x, projectAreaSize.y * targetRect.anchorMin.y) + parentOffset;

        var viewportPos = targetCamera.WorldToViewportPoint(worldPos);
        return new Vector2(projectAreaSize.x * viewportPos.x, projectAreaSize.y * viewportPos.y) - projectDistOffset;
    }

    //Şu an için parent anchor ın stretch-stretch veya middle-center olmasını destekliyor (Diğerleri GetOffsetCausedByParentAnchoring() daki sizeDelta ve offset ler yüzünden bozuluyor)
    public static Vector2 ScreenToCanvasPos(RectTransform targetRect, Vector2 screenPos, Camera targetCamera, Canvas targetCanvas, bool projectInLocalSpace)
    {
        RectTransform projectTransform = GetProjectionTransform(targetRect, targetCanvas, projectInLocalSpace);

        Vector2 parentOffset = Vector2.zero;
        if (CheckIfProjectInWorldAndParentIsNotCanvas(targetRect,targetCanvas, projectInLocalSpace))
            parentOffset = GetOffsetCausedByParentAnchoring(targetRect);

        var projectAreaSize = projectTransform.rect.size;
        Vector2 projectDistOffset = new Vector2(projectAreaSize.x * targetRect.anchorMin.x, projectAreaSize.y * targetRect.anchorMin.y) + parentOffset;

        var viewportPos = targetCamera.ScreenToViewportPoint(screenPos);
        return new Vector2(projectAreaSize.x * viewportPos.x, projectAreaSize.y * viewportPos.y) - projectDistOffset;
    }

    static RectTransform GetProjectionTransform(RectTransform targetRect, Canvas targetCanvas, bool projectInLocalSpace)
    {
        RectTransform projectTransform = (RectTransform)targetCanvas.transform;
        if (projectInLocalSpace)
            projectTransform = (RectTransform)targetRect.parent;

        return projectTransform;
    }

    static Vector2 GetOffsetCausedByParentAnchoring(RectTransform targetRect)
    {
        Vector2 offset = Vector2.zero;

        var parentRect = (RectTransform)targetRect.parent;

        offset.x = (parentRect.sizeDelta.x / 2f) + parentRect.offsetMin.x;
        offset.y = -1 * ((parentRect.sizeDelta.y / 2f) + (-1 * parentRect.offsetMax.y));

        return offset;
    }

    static bool CheckIfProjectInWorldAndParentIsNotCanvas(RectTransform targetRect, Canvas targetCanvas, bool projectInLocalSpace)
    {
        return !projectInLocalSpace && (RectTransform)targetRect.parent != (RectTransform)targetCanvas.transform;
    }
}