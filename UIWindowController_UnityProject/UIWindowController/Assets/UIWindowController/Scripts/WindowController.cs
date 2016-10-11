using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public abstract class WindowController: MonoBehaviour
{
    [HideInInspector]
    public int WindowType;

    public MMTweenAlpha BGFilter;
    public MMTweenPosition PositionTween;

    protected IEnumerator _deactivateRoutine, _activateRoutine;

    public bool IsPreActivationFinished{ get; protected set; }

    public bool IsPostActivationFinished { get; protected set; }

    public bool IsPreDeactivationFinished { get; protected set; }

    public bool IsPostDeactivationFinished { get; protected set; }

    public Action OnPreActivationFinished, OnPostActivationFinished, OnPreDeactivationFinished, OnPostDeactivationFinished;
    
    public virtual void Init()
    {
        _deactivateRoutine = null;
        _activateRoutine = null;

        gameObject.SetActive(false);

        if (PositionTween == null)
            transform.localPosition = Vector3.zero;
        //else
        //    transform.position = PositionTween.From;

        IsPreDeactivationFinished = true;
        IsPostDeactivationFinished = true;

        IsPreActivationFinished = false;
        IsPostActivationFinished = false;
    }

    public virtual void Activate(int sortOrder)
    {
        gameObject.SetActive(true);

        if (_activateRoutine != null)
            StopCoroutine(_activateRoutine);

        _activateRoutine = ActivateRoutine(sortOrder);
        StartCoroutine(_activateRoutine);
    }

    IEnumerator ActivateRoutine(int sortOrder)
    {
        IsPreDeactivationFinished = false;
        IsPostDeactivationFinished = false;

        yield return StartCoroutine(PreActivateAdditional());
        IsPreActivationFinished = true;

        if (OnPreActivationFinished != null)
            OnPreActivationFinished();

        SetSortOrder(sortOrder);

        if (PositionTween != null)
        {
            PositionTween.PlayForward();

            yield return new WaitForSeconds(PositionTween.Duration);
        }

        yield return StartCoroutine(PostActivateAdditional());
        IsPostActivationFinished = true;

        if (OnPostActivationFinished != null)
            OnPostActivationFinished();
    }

    public void SetSortOrder(int sortOrder)
    {
        //TODO: Burda hierarchy de aşağı veya yukarı yerini değiştirerek öne veya arkaya geçirme işlemi yapılmalı
    }

    /// <summary>
    /// NEVER call this method, this method is called from GeneralWindowController.
    /// INSTEAD call CloseWindow() method.
    /// </summary>
    /// <param name="closeImmediately"></param>
    public virtual void Deactivate(bool closeImmediately = false)
    {
        if (_deactivateRoutine != null)
            StopCoroutine(_deactivateRoutine);

        if (gameObject.activeSelf)
        {
            _deactivateRoutine = DeactivateRoutine(closeImmediately);
            StartCoroutine(_deactivateRoutine);
        }
    }

    IEnumerator DeactivateRoutine(bool closeImmediately)
    {
        IsPreActivationFinished = false;
        IsPostActivationFinished = false;

        yield return StartCoroutine(PreDeactivateAdditional());
        IsPreDeactivationFinished = true;

        if (OnPreDeactivationFinished != null)
            OnPreDeactivationFinished();

        if (!closeImmediately)
        {
            if (PositionTween != null)
            {
                PositionTween.PlayReverse();

                yield return new WaitForSeconds(PositionTween.Duration);

                //if(Time.timeScale != 1)
                //    yield return new WaitWhile(() => PositionTween.value != PositionTween.from);
                //else
                //    yield return new WaitForSeconds(PositionTween.Duration);
            }
        }

        yield return StartCoroutine(PostDeactivateAdditional());
        IsPostDeactivationFinished = true;

        if (OnPostDeactivationFinished != null)
            OnPostDeactivationFinished();

        gameObject.SetActive(false);
    }

    protected void SetBGFilter(bool enable)
    {
        if (BGFilter == null)
            return;

        BGFilter.gameObject.SetActive(true);

        if (enable)
            BGFilter.PlayForward();
        else
            BGFilter.PlayReverse();
    }

    protected virtual IEnumerator PreDeactivateAdditional()
    {
        SetBGFilter(false);

        yield return new WaitForEndOfFrame();
    }

    protected virtual IEnumerator PostDeactivateAdditional()
    {
        yield return new WaitForEndOfFrame();
    }

    protected virtual IEnumerator PreActivateAdditional()
    {
        SetBGFilter(true);

        yield return new WaitForEndOfFrame();
    }

    protected virtual IEnumerator PostActivateAdditional()
    {
        yield return new WaitForEndOfFrame();
    }

    protected static void ActivateOnOffSprite(Image onImage, Image offImage, bool isOn)
    {
        onImage.gameObject.SetActive(isOn);
        offImage.gameObject.SetActive(!isOn);
    }
}
