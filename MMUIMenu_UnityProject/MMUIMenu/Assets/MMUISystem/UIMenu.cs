using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class UIMenu : MonoBehaviour
{
    [Tooltip("Disable menus that are under this one in the stack")]
    public bool DisableMenusUnderneath;
    [Tooltip("Use Intro tweeners as outro tweeners as well")]
    public bool UseIntroTweenersAsOutro;
    public List<MMUITweener> IntroTweeners, OutroTweeners;
    
    public bool IsPreActivationFinished { get; protected set; }
    public bool IsPostActivationFinished { get; protected set; }
    public bool IsPreDeactivationFinished { get; protected set; }
    public bool IsPostDeactivationFinished { get; protected set; }
    public Canvas Canvas { get; protected set; }

    public static Action<UIMenu> OnPreActivation, OnPostActivation, OnPreDeactivation, OnPostDeactivation;

    protected List<UnityUIButton> UIButtons;
    protected IEnumerator _deactivateRoutine, _activateRoutine;

    private int _finishedTweenerCount;

    protected virtual IEnumerator PreDeactivateAdditional()
    {
        FinishListeningEvents();

        yield return new WaitForEndOfFrame();
    }

    protected virtual IEnumerator PostDeactivateAdditional()
    {
        yield return new WaitForEndOfFrame();
    }

    protected virtual IEnumerator PreActivateAdditional()
    {
        yield return new WaitForEndOfFrame();
    }

    protected virtual IEnumerator PostActivateAdditional()
    {
        StartListeningEvents();

        yield return new WaitForEndOfFrame();
    }

    protected virtual void Awake()
    {
        Canvas = GetComponent<Canvas>();

        Init();
    }

    protected virtual void OnDestroy()
    {
        IntroTweeners.ForEach(t => t.ResetEventDelegates());

        if (!UseIntroTweenersAsOutro)
            OutroTweeners.ForEach(t => t.ResetEventDelegates());
    }

    protected void Init()
    {
        gameObject.SetActive(false);

        IsPreDeactivationFinished = true;
        IsPostDeactivationFinished = true;

        IsPreActivationFinished = false;
        IsPostActivationFinished = false;

        UIButtons = new List<UnityUIButton>(GetComponentsInChildren<UnityUIButton>(true));

        _finishedTweenerCount = 0;
    }

    #region Activation / Deactivation
    public virtual void Activate()
    {
        gameObject.SetActive(true);

        if (_deactivateRoutine != null)
            StopCoroutine(_deactivateRoutine);

        if (_activateRoutine != null)
            StopCoroutine(_activateRoutine);

        _activateRoutine = ActivateRoutine();
        StartCoroutine(_activateRoutine);
    }

    IEnumerator ActivateRoutine()
    {
        IsPreDeactivationFinished = false;
        IsPostDeactivationFinished = false;

        yield return StartCoroutine(PreActivateAdditional());
        IsPreActivationFinished = true;

        if (OnPreActivation != null)
            OnPreActivation(this);

        IntroTweeners.ForEach(tw => tw.InitValueToFROM());

        PlayTweeners(IntroTweeners, true);
        
        yield return new WaitUntil(() => _finishedTweenerCount == IntroTweeners.Count);

        yield return StartCoroutine(PostActivateAdditional());
        IsPostActivationFinished = true;

        if (OnPostActivation != null)
            OnPostActivation(this);
    }

    public virtual void Deactivate()
    {
        if (_activateRoutine != null)
            StopCoroutine(_activateRoutine);

        if (_deactivateRoutine != null)
            StopCoroutine(_deactivateRoutine);

        _deactivateRoutine = DeactivateRoutine();
        StartCoroutine(_deactivateRoutine);
    }

    IEnumerator DeactivateRoutine()
    {
        IsPreActivationFinished = false;
        IsPostActivationFinished = false;

        yield return StartCoroutine(PreDeactivateAdditional());
        IsPreDeactivationFinished = true;

        if (OnPreDeactivation != null)
            OnPreDeactivation(this);

        if (!UseIntroTweenersAsOutro)
            OutroTweeners.ForEach(tw => tw.InitValueToFROM());

        var targetTweeners = UseIntroTweenersAsOutro ? IntroTweeners : OutroTweeners;
        PlayTweeners(targetTweeners, false);

        yield return new WaitUntil(() => _finishedTweenerCount == targetTweeners.Count);

        yield return StartCoroutine(PostDeactivateAdditional());
        IsPostDeactivationFinished = true;

        gameObject.SetActive(false);

        if (OnPostDeactivation != null)
            OnPostDeactivation(this);
    }

    private void OnTweenFinished()
    {
        _finishedTweenerCount++;
    }
    #endregion

    private void PlayTweeners(List<MMUITweener> targetTweeners, bool isOpening)
    {
        _finishedTweenerCount = 0;

        if (targetTweeners != null && targetTweeners.Count > 0)
        {
            targetTweeners.ForEach(t => t.ResetEventDelegates());

            if (isOpening)
                targetTweeners.ForEach(tw => tw.PlayForward()); //targetTweeners : IntroTweens
            else
            {
                if(UseIntroTweenersAsOutro)
                    targetTweeners.ForEach(tw => tw.PlayReverse()); //targetTweeners : IntroTweens
                else
                    targetTweeners.ForEach(tw => tw.PlayForward()); //targetTweeners : OutroTweens
            }

            targetTweeners.ForEach(t => t.AddOnFinish(OnTweenFinished, false));
        }
    }

    public abstract void OnBackPressed(PointerEventData eventData);
    protected abstract void FinishListeningEvents();
    protected abstract void StartListeningEvents();
}
