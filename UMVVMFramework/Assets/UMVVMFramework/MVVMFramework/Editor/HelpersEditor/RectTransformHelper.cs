using UnityEditor;
using UnityEngine;

public static class RectTransformHelper
{
    [MenuItem("RectTransform/Set Anchors To Cur Rect %&t", true)]
    static bool ValidateSetRectTransformAnchorsToCurRect()
    {
        foreach(var go in Selection.gameObjects)
        {
            if (go == null || go.GetComponent<RectTransform>() == null || go.transform.parent == null)
                return false;
        }

        return true;
    }

    [MenuItem("RectTransform/Set Anchors To Cur Rect %&t")]
    static void SetRectTransformAnchorsToCurRect()
    {
        foreach(GameObject go in Selection.gameObjects)
        {
            RectTransform selectedRectTrans = go.GetComponent<RectTransform>();
            RectTransform parentRectTrans = selectedRectTrans.parent.GetComponent<RectTransform>();

            var offsetMin = selectedRectTrans.offsetMin;
            var offsetMax = selectedRectTrans.offsetMax;
            var curAnchorMin = selectedRectTrans.anchorMin;
            var curAnchorMax = selectedRectTrans.anchorMax;

            var parent_width = parentRectTrans.rect.width;
            var parent_height = parentRectTrans.rect.height;

            var anchorMin = new Vector2(curAnchorMin.x + (offsetMin.x / parent_width), curAnchorMin.y + (offsetMin.y / parent_height));
            var anchorMax = new Vector2(curAnchorMax.x + (offsetMax.x / parent_width), curAnchorMax.y + (offsetMax.y / parent_height));

            selectedRectTrans.anchorMin = anchorMin;
            selectedRectTrans.anchorMax = anchorMax;

            selectedRectTrans.offsetMin = new Vector2(0, 0);
            selectedRectTrans.offsetMax = new Vector2(0, 0);
        }
    }
}
