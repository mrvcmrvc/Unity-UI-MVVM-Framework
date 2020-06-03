using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MVVM
{
    public class FloatingJoystick : Joystick
    {
        private Canvas _canvas;
        private CanvasScaler _scaler;
        public RectTransform _safeArea;

        public bool AlwaysVisible;
        public float JoystickBaseMoveThreshold;
        public float BaseFixedDistance;

        protected override void Start()
        {
            base.Start();

            _safeArea = transform.parent.GetComponent<RectTransform>();
            _canvas = GetComponentInParent<Canvas>();
            _scaler = GetComponentInParent<CanvasScaler>();

            SetJoystickPos(new Vector3(Screen.width / 2.0f, Screen.height * BaseFixedDistance));

            if (!AlwaysVisible)
                background.gameObject.SetActive(false);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            SetJoystickPos(eventData.position);

            background.gameObject.SetActive(true);

            base.OnPointerDown(eventData);
        }

        private void SetJoystickPos(Vector3 screenPos)
        {
            Vector3 vpPoint = Camera.main.ScreenToViewportPoint(screenPos);

            //float bottomDist = -1 * _canvas.pixelRect.height * _safeArea.anchorMin.y;

            //Vector2 projectionArea = _safeArea.rect.size;
            //projectionArea.y += bottomDist;

            background.anchoredPosition = ((RectTransform)_canvas.transform).rect.size * vpPoint;
        }

        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);

            bool updateJoystickPos = CheckForFingerDistance(eventData, out float deltaMove);

            if (updateJoystickPos)
                MoveJoystickTowardsPos(eventData.position, deltaMove);
        }

        private bool CheckForFingerDistance(PointerEventData eventData, out float deltaMove)
        {
            Vector2 joystickScreenPos = RectTransformUtility.WorldToScreenPoint(Cam, background.position);

            float curDist = Vector2.Distance(joystickScreenPos, eventData.position);

            float scaledThreshold = JoystickBaseMoveThreshold * (Screen.width / _scaler.referenceResolution.x);

            deltaMove = curDist - scaledThreshold;
            if (deltaMove < 0)
                deltaMove = 0;

            if (curDist >= scaledThreshold)
                return true;

            return false;
        }

        private void MoveJoystickTowardsPos(Vector2 pressPosition, float deltaMove)
        {
            Vector2 joystickScreenPos = RectTransformUtility.WorldToScreenPoint(Cam, background.position);

            Vector2 dir = (pressPosition - joystickScreenPos).normalized;

            Vector2 movement = dir * deltaMove;

            Vector2 newScreenPos = joystickScreenPos + movement;

            SetJoystickPos(newScreenPos);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            if (!AlwaysVisible)
                background.gameObject.SetActive(false);

            SetJoystickPos(new Vector3(Screen.width / 2.0f, Screen.height * BaseFixedDistance));

            base.OnPointerUp(eventData);
        }
    }
}