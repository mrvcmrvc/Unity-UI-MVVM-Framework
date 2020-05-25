using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace MVVM
{
    public class NumericalCounterScript : MonoBehaviour
    {
        public float Speed;
        public int StepSize;

        private TextMeshProUGUI _textMesh;
        public TextMeshProUGUI TextMesh
        {
            get
            {
                if (_textMesh == null)
                    _textMesh = GetComponent<TextMeshProUGUI>();

                return _textMesh;
            }
        }

        public int TargetValue { get; private set; }
        public int StartingValue { get; private set; }

        private IEnumerator _counterRoutine;
        private Action _callback;
        private bool _usePrefix;

        private void Awake()
        {
            if (Speed == 0)
                Speed = 1;
            if (StepSize == 0)
                StepSize = 1;
        }

        public void SetCounter(int value, bool instant, bool usePrefix, Action callback)
        {
            if (_counterRoutine != null)
                StopCoroutine(_counterRoutine);

            if (instant)
                SetInstantly(value, usePrefix, callback);
            else
                PrepareRoutine(value, usePrefix, callback);
        }

        public void FinishCounting(bool usePrefix, Action callback)
        {
            if (_counterRoutine != null)
                StopCoroutine(_counterRoutine);

            if (callback != null)
                _callback = callback;

            string prefix = "";
            if (usePrefix)
                prefix = GetPrefix(TargetValue);

            TextMesh.SetText(prefix + TargetValue);

            FireOnCountFinished();
        }

        private void PrepareRoutine(int value, bool usePrefix, Action callback)
        {
            _callback = callback;

            int startingValue;
            if (!int.TryParse(TextMesh.text, out startingValue))
            {
                Debug.LogError("Could not parse text value to int for counter of " + gameObject.name + ", setting it to 0");

                StartingValue = 0;
            }
            else
                StartingValue = startingValue;

            TargetValue = value;

            _usePrefix = usePrefix;

            _counterRoutine = CounterRoutine();
            StartCoroutine(_counterRoutine);
        }

        private void SetInstantly(int value, bool usePrefix, Action callback)
        {
            string prefix = "";
            if (usePrefix)
                prefix = GetPrefix(value);

            TextMesh.SetText(prefix + value);

            FireOnCountFinished();
        }

        private IEnumerator CounterRoutine()
        {
            int sign = TargetValue < StartingValue ? -1 : 1;

            while (StartingValue != TargetValue)
            {
                StartingValue += sign * StepSize;

                string prefix = "";
                if (_usePrefix)
                    prefix = GetPrefix(StartingValue);

                TextMesh.SetText(prefix + StartingValue);

                yield return new WaitForSeconds(Time.deltaTime / Speed);
            }

            _counterRoutine = null;

            FireOnCountFinished();
        }

        private string GetPrefix(int value)
        {
            if (value > 0)
                return "+";
            else if (value == 0)
                return "";
            else
                return "-";
        }

        private void FireOnCountFinished()
        {
            if (_callback != null)
                _callback.Invoke();
        }
    }
}