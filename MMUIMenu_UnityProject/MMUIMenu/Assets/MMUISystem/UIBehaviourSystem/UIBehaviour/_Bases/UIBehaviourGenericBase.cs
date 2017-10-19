using System;

public abstract class UIBehaviourBase<T> : UIBehaviourBase where T : UIBehaviourBase<T>
{
    public Action<bool> OnTweenFinished;
    protected void FireOnTweenFinished(bool isActive)
    {
        if (OnTweenFinished != null)
            OnTweenFinished(isActive);
    }
}
