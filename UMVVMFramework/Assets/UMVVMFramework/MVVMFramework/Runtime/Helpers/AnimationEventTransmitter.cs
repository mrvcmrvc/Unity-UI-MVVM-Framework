using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

namespace MVVM
{
    [System.Serializable]
    public class AnimationEventBinder
    {
        public int AnimEventID;
        public UnityEvent Event;

        public void Callback()
        {
            Event.Invoke();
        }
    }

    public class AnimationEventTransmitter : MonoBehaviour
    {
        public bool IsDebugEnabled;

        public List<AnimationEventBinder> EventBinderList;

        public void TriggerEvent(int id)
        {
            EventBinderList.Single(val => val.AnimEventID == id).Callback();
        }
    }
}