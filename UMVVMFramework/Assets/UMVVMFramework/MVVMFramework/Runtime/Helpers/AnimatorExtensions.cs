using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

namespace MVVM
{
    public static class AnimatorExtentions
    {
        public class WaitCallback
        {
            public IEnumerator Routine;
            public Action Callback;

            public WaitCallback(IEnumerator routine, Action callback)
            {
                Routine = routine;
                Callback = callback;
            }
        }

        static Dictionary<Animator, WaitCallback> _animatorDict;

        static AnimatorExtentions()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        }

        static void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode loadMode)
        {
            CleanUpAnimatorDict();
        }

        static void CleanUpAnimatorDict()
        {
            if (_animatorDict == null)
                return;

            for (int i = 0; i < _animatorDict.Count; i++)
            {
                var kvp = _animatorDict.ElementAt(i);

                if (kvp.Key == null)
                {
                    _animatorDict.Remove(kvp.Key);
                    i--;
                }
            }
        }

        public static void OnComplete(this Animator animator, MonoBehaviour behaviour, Action callback)
        {
            if (_animatorDict == null)
                _animatorDict = new Dictionary<Animator, WaitCallback>();

            WaitCallback existingCallback = null;

            if (_animatorDict.TryGetValue(animator, out existingCallback))
            {
                existingCallback.Callback += callback;
                return;
            }

            if (!behaviour.gameObject.activeInHierarchy)
                return;

            IEnumerator waitForAnimCompleteRoutine = WaitForAnimComplete(animator, callback);

            _animatorDict.Add(animator, new WaitCallback(waitForAnimCompleteRoutine, callback));

            behaviour.StartCoroutine(waitForAnimCompleteRoutine);
        }

        static IEnumerator WaitForAnimComplete(this Animator animator, Action callback)
        {
            yield return new WaitForSecondsRealtime(Time.fixedUnscaledDeltaTime);


            while (animator && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
                yield return null;

            if (!_animatorDict.ContainsKey(animator))
                yield break;

            WaitCallback waitCallback = _animatorDict[animator];

            _animatorDict.Remove(animator);

            if (waitCallback.Callback != null)
                waitCallback.Callback();
        }

        public static void UnregisterOnComplete(this Animator animator, MonoBehaviour behaviour, Action callback)
        {
            if (_animatorDict == null)
                return;

            WaitCallback existingCallback = null;

            if (_animatorDict.TryGetValue(animator, out existingCallback))
            {
                existingCallback.Callback -= callback;

                if (existingCallback.Callback == null)
                    _animatorDict.Remove(animator);
            }
        }

        public static void UnregisterOnComplete(this Animator animator, MonoBehaviour behaviour)
        {
            if (_animatorDict == null)
                return;

            WaitCallback existingCallback = null;

            if (_animatorDict.TryGetValue(animator, out existingCallback))
            {
                behaviour.StopCoroutine(existingCallback.Routine);
                _animatorDict.Remove(animator);
            }
        }

        public static bool ContainsParam(this Animator animator, string paramName)
        {
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                if (param.name == paramName)
                    return true;
            }

            return false;
        }
    }
}