using UnityEngine;

namespace MVVM
{
    public enum ERectTransEnvelopeMode
    {
        AlwaysUpdate,
        OnlyOnChange,
    }

    [ExecuteInEditMode]
    public class RectTransfromEnvelopeElement : MonoBehaviour
    {
        public RectTransform FromTransform;
        public RectTransform ToTransform;

        public ERectTransEnvelopeMode UpdateMode;

        public float XOffset, YOffset;

        private void Awake()
        {
            if (ToTransform == null || FromTransform == null)
                return;

            UpdateAnchors();
        }

        private void UpdateAnchors()
        {
            ToTransform.anchorMin = FromTransform.anchorMin;
            ToTransform.anchorMax = FromTransform.anchorMax;
        }

        private void LateUpdate()
        {
            if (FromTransform == null || ToTransform == null)
                return;

            if (!CheckForUpdateMode())
                return;

            UpdateAnchors();

            Rect fromRect = FromTransform.rect;

            ToTransform.sizeDelta = new Vector2(fromRect.width - XOffset, fromRect.height - YOffset);
            ToTransform.anchoredPosition = FromTransform.anchoredPosition;
        }

        private bool CheckForUpdateMode()
        {
            switch (UpdateMode)
            {
                case ERectTransEnvelopeMode.OnlyOnChange:
                    return FromTransform.hasChanged;
                case ERectTransEnvelopeMode.AlwaysUpdate:
                    return true;
                default:
                    return true;
            }
        }
    }
}