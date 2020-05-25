using UnityEngine;
using UnityEngine.UI;

namespace MVVM
{
    [RequireComponent(typeof(Graphic))]
    public class DistanceBasedAlphaSetter : MonoBehaviour
    {
        public Transform TargetTransform;
        public float Coef;
        public float MinAlpha;
        public float MaxAlpha;

        private Graphic _graphic;
        private float _initialDist;

        private void Start()
        {
            if (_graphic != null)
                return;

            _graphic = GetComponent<Graphic>();

            _initialDist = Vector3.Distance(transform.position, TargetTransform.position);
        }

        private void OnEnable()
        {
            Update();
        }

        private void Update()
        {
            if (_graphic == null)
                return;

            Color curColor = _graphic.color;

            float curDist = Vector3.Distance(transform.position, TargetTransform.position);
            float distCoef = curDist / _initialDist;
            if (distCoef > 1)
                distCoef = 1.0f;

            distCoef = 1 - distCoef;

            curColor.a = MinAlpha + (distCoef * Coef * (MaxAlpha - MinAlpha));

            _graphic.color = curColor;
        }
    }
}