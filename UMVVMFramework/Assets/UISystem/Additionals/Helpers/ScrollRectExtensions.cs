using System;
using UnityEngine;
using UnityEngine.UI;
using UITweening;

namespace MVVM
{
    public static class ScrollRectExtensions
    {
        public static void RenderTargetRectElementFully(this ScrollRect targetScrollRect, Rect elementRect, Vector2 pivot, float scaleFactor, Action<float> callback)
        {
            float deltaAmount = CheckForBottom(targetScrollRect, elementRect, pivot, scaleFactor);
            if (deltaAmount > 0f)
                SetTweener(targetScrollRect, deltaAmount, callback);

            deltaAmount = CheckForTop(targetScrollRect, elementRect, pivot, scaleFactor);
            if (deltaAmount < 0f)
                SetTweener(targetScrollRect, deltaAmount, null);
        }

        private static float CheckForTop(ScrollRect targetScrollRect, Rect elementRect, Vector2 pivot, float scaleFactor)
        {
            RectTransform targetScrollAreaTrans = (RectTransform)targetScrollRect.transform;
            Rect targetScrollRectInWorld = targetScrollAreaTrans.GetRect(scaleFactor);

            Vector3 elementRectTopPos = elementRect.max;
            elementRectTopPos.x = elementRect.center.x;

            if (!targetScrollAreaTrans.RectContainsPoints(scaleFactor, elementRectTopPos))
            {
                Vector3 targetScrollRectTopPos = targetScrollRectInWorld.max;
                targetScrollRectTopPos.x = targetScrollRectInWorld.center.x;

                return targetScrollRectTopPos.y - elementRectTopPos.y;
            }

            return 0f;
        }

        private static float CheckForBottom(ScrollRect targetScrollRect, Rect elementRect, Vector2 pivot, float scaleFactor)
        {
            RectTransform targetScrollAreaTrans = (RectTransform)targetScrollRect.transform;
            Rect targetScrollRectInWorld = targetScrollAreaTrans.GetRect(scaleFactor);

            Vector3 elementRectBottomPos = elementRect.min;
            elementRectBottomPos.x = elementRect.center.x;

            if (!targetScrollAreaTrans.RectContainsPoints(scaleFactor, elementRectBottomPos))
            {
                Vector3 targetScrollRectBottomPos = targetScrollRectInWorld.min;
                targetScrollRectBottomPos.x = targetScrollRectInWorld.center.x;

                return targetScrollRectBottomPos.y - elementRectBottomPos.y;
            }

            return 0f;
        }

        private static void SetTweener(ScrollRect targetScrollRect, float deltaAmount, Action<float> callback)
        {
            targetScrollRect.enabled = false;

            Action<float, bool> onTweenerUpdate = delegate (float value, bool isFinished)
            {
                if (isFinished)
                {
                    if (callback != null)
                        callback(deltaAmount);

                    targetScrollRect.enabled = true;
                }

                Vector3 curPos = targetScrollRect.content.position;
                curPos.y = value;

                targetScrollRect.content.position = curPos;
            };

            UIVirtualValueTweenerInfo tweenerInfo = new UIVirtualValueTweenerInfo
            {
                Callback = onTweenerUpdate,
                Duration = 0.5f,
                Ease = UITweeningEaseEnum.OutCubic,
                From = targetScrollRect.content.position.y,
                To = targetScrollRect.content.position.y + deltaAmount,
                IgnoreTimeScale = true
            };

            UIVirtualValueTweener newTweener = new UIVirtualValueTweener(tweenerInfo);

            UIVirtualValueTweenController.Instance.StartTweener(newTweener);
        }
    }
}