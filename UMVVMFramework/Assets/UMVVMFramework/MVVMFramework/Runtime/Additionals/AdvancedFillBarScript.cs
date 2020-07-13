using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdvancedFillBarScript : MonoBehaviour
{
    #region Events
    public Action<float> OnBarUpdate;
    private void FireOnBarUpdate(float clampedPassedTime)
    {
        OnBarUpdate?.Invoke(clampedPassedTime);
    }

    public Action OnBarUpdateComplete;
    private void FireOnBarUpdateComplete()
    {
        OnBarUpdateComplete?.Invoke();
    }
    #endregion

    public Slider Slider;
    public Image IncreaseFill, DecreaseFill;
    public Image FillObj;
    public TextMeshProUGUI PercentageText;
    public RectTransform PlaceholderGrid;
    public UISpawnController SpawnController;

    [Range(0f, 1f)]
    public float InitialValue;
    public float Duration;
    public int ValuePerCell;

    public bool UseDeltaColors = true;
    public bool ShowAsPercentage;
    public bool ShowGrids;
    public bool BarGradientColor;

    public Color DecreaseColor = new Color(207.0f / 255.0f, 42.0f / 255.0f, 39.0f / 255.0f), IncreaseColor = new Color(0.0f / 255.0f, 158.0f / 255.0f, 15.0f / 255.0f);
    public Color FullColor = Color.white, EmptyColor = Color.white;
    public bool LerpH = true, LerpS = true, LerpV = true;

    private RectTransform _incRect, _decRect;
    private RectTransform IncRect
    {
        get
        {
            if (_incRect == null)
                _incRect = IncreaseFill.GetComponent<RectTransform>();

            return _incRect;
        }
    }

    private RectTransform DecRect
    {
        get
        {
            if (_decRect == null)
                _decRect = DecreaseFill.GetComponent<RectTransform>();

            return _decRect;
        }
    }

    IEnumerator _updateRoutine;
    float _targetValue;
    bool _isBarDecrease;
    List<Image> _gridObjs = new List<Image>();
    float _fillRectWidth, _fillRectHeight;

    private void Awake()
    {
        _updateRoutine = null;
        _isBarDecrease = false;

        CalculateOrgRectWidthHeight();

        _targetValue = InitialValue;
        Slider.value = InitialValue;

        IncreaseFill.color = IncreaseColor;
        DecreaseFill.color = DecreaseColor;

        if (BarGradientColor)
        {
            UseDeltaColors = false;

            UpdateGradientColor();
        }

        if (UseDeltaColors)
            BarGradientColor = false;
    }

    private void CalculateOrgRectWidthHeight()
    {
        Slider.value = 1f;

        Canvas.ForceUpdateCanvases();

        _fillRectWidth = Slider.fillRect.rect.width;
        _fillRectHeight = Slider.fillRect.rect.height;
    }

    private void Start()
    {
        TooglePercentage(ShowAsPercentage);
    }

    public void UpdateBar(int percentage, bool instant = false)
    {
        if (_updateRoutine != null)
            StopCoroutine(_updateRoutine);

        _targetValue = percentage / 100.0f;

        UpdatePercentageText(percentage);

        _isBarDecrease = Slider.value > _targetValue;

        if (!instant)
        {
            _updateRoutine = UpdateSliderValue();
            StartCoroutine(_updateRoutine);
        }
        else
        {
            ResetIncDecBarAnchor();

            Slider.value = _targetValue;

            if (BarGradientColor)
                UpdateGradientColor();

            FireOnBarUpdateComplete();
        }
    }

    public void TooglePercentage(bool isActive)
    {
        ShowAsPercentage = isActive;

        PercentageText.gameObject.SetActive(ShowAsPercentage);
    }

    public void ActivateGrids(int maxValue)
    {
        if (!CheckIfGridDrawCond(maxValue))
            return;

        ShowGrids = true;

        InitGridPivot();

        InitGrids(maxValue);

        OrderGrids();
    }

    public void DeactivateGrids()
    {
        ShowGrids = false;

        if (_gridObjs.Count != 0)
        {
            for (int i = _gridObjs.Count - 1; i > 0; i--)
                Destroy(_gridObjs[i].gameObject);
        }

        _gridObjs.Clear();
    }

    private void UpdatePercentageText(float percentage)
    {
        PercentageText.text = "%" + percentage.ToString();
    }

    private bool CheckIfGridDrawCond(int maxValue)
    {
        if (maxValue < ValuePerCell)
        {
            DeactivateGrids();

            return false;
        }

        return true;
    }

    private void InitGridPivot()
    {
        Vector2 gridPivot = new Vector2(0.5f, 0.5f);

        if (Slider.direction == Slider.Direction.LeftToRight)
            gridPivot.x = 0f;
        else if (Slider.direction == Slider.Direction.RightToLeft)
            gridPivot.x = 1f;
        else if (Slider.direction == Slider.Direction.BottomToTop)
            gridPivot.y = 0f;
        else if (Slider.direction == Slider.Direction.TopToBottom)
            gridPivot.y = 1f;

        PlaceholderGrid.SetPivotWithCounterAdjustPosition(gridPivot);
    }

    private void InitGrids(int maxValue)
    {
        if (_gridObjs.Count != 0)
            DeactivateGrids();

        int totalCellCount = maxValue / ValuePerCell;
        int gridLineCount = totalCellCount;
        _gridObjs = SpawnController.LoadSpawnables<Image>(gridLineCount, true);
    }

    private float CalculateCellSizeOnAppAxis(int gridLineCount)
    {
        float areaSize = GetAppropriateFillRectSize();
        float gridSize = GetAppropriateGridSize();

        float useableArea = areaSize - (gridLineCount * gridSize);

        return useableArea / gridLineCount;
    }

    private float GetAppropriateFillRectSize()
    {
        if (_fillRectHeight == 0 || _fillRectWidth == 0)
            CalculateOrgRectWidthHeight();

        if (Slider.direction == Slider.Direction.BottomToTop || Slider.direction == Slider.Direction.TopToBottom)
            return _fillRectHeight;

        return _fillRectWidth;
    }

    private float GetAppropriateGridSize()
    {
        if (Slider.direction == Slider.Direction.BottomToTop || Slider.direction == Slider.Direction.TopToBottom)
            return PlaceholderGrid.rect.height;

        return PlaceholderGrid.rect.width;
    }

    private void OrderGrids()
    {
        int sign = 1;
        if (Slider.direction == Slider.Direction.RightToLeft || Slider.direction == Slider.Direction.TopToBottom)
            sign = -1;

        float cellSize = CalculateCellSizeOnAppAxis(_gridObjs.Count);

        float offset = 0f;
        for (int i = 0; i < _gridObjs.Count; i++)
        {
            if (i != 0)
                offset = GetAppropriateGridSize();

            var newPos = _gridObjs[i].transform.localPosition;

            if (Slider.direction == Slider.Direction.LeftToRight || Slider.direction == Slider.Direction.RightToLeft)
                newPos.x = sign * (((i + 1) * cellSize) + (i * offset));
            else
                newPos.y = sign * (((i + 1) * cellSize) + (i * offset));

            ((RectTransform)_gridObjs[i].transform).anchoredPosition = newPos;
        }
    }

    IEnumerator UpdateSliderValue()
    {
        float from = Slider.value;
        float to = _targetValue;
        float passedTime = 0f;

        ResetIncDecBarAnchor();

        while (passedTime <= Duration)
        {
            Slider.value = Mathf.Lerp(from, to, passedTime / Duration);

            if (UseDeltaColors)
                SetIncDecBarAnchors();
            else if (BarGradientColor)
                UpdateGradientColor();

            FireOnBarUpdate(passedTime / Duration);

            passedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        Slider.value = to;

        _updateRoutine = null;

        FireOnBarUpdateComplete();
    }

    private void UpdateGradientColor()
    {
        float h1, s1, v1, h2, s2, v2, h3, s3, v3;
        Color.RGBToHSV(EmptyColor, out h1, out s1, out v1);
        Color.RGBToHSV(FullColor, out h2, out s2, out v2);

        h3 = LerpH ? Mathf.Lerp(h1, h2, Slider.value) : h1;
        s3 = LerpS ? Mathf.Lerp(s1, s2, Slider.value) : s1;
        v3 = LerpV ? Mathf.Lerp(v1, v2, Slider.value) : v1;

        FillObj.color = Color.HSVToRGB(h3, s3, v3);
    }

    void SetIncDecBarAnchors()
    {
        Vector2 anchorMax, anchorMin;

        ResetIncDecBarAnchor();

        if (_isBarDecrease)
        {
            anchorMax = DecRect.anchorMax;
            anchorMin = DecRect.anchorMin;
        }
        else
        {
            anchorMax = IncRect.anchorMax;
            anchorMin = IncRect.anchorMin;
        }

        switch (Slider.direction)
        {
            case Slider.Direction.LeftToRight:
            case Slider.Direction.BottomToTop:
                if (_isBarDecrease)
                {
                    anchorMax.x = Slider.value;
                    anchorMin.x = _targetValue;
                }
                else
                {
                    anchorMax.x = _targetValue;
                    anchorMin.x = Slider.value;
                }
                break;
            case Slider.Direction.RightToLeft:
            case Slider.Direction.TopToBottom:
                if (_isBarDecrease)
                {
                    anchorMin.x = 1 - Slider.value;
                    anchorMax.x = 1 - _targetValue;
                }
                else
                {
                    anchorMin.x = 1 - _targetValue;
                    anchorMax.x = 1 - Slider.value;
                }
                break;
        }

        if (_isBarDecrease)
        {
            DecRect.anchorMax = anchorMax;
            DecRect.anchorMin = anchorMin;
        }
        else
        {
            IncRect.anchorMax = anchorMax;
            IncRect.anchorMin = anchorMin;
        }
    }

    private void ResetIncDecBarAnchor()
    {
        switch (Slider.direction)
        {
            case Slider.Direction.LeftToRight:
                if (_isBarDecrease)
                {
                    IncRect.anchorMin = new Vector2(1f, 0f);
                    IncRect.anchorMax = new Vector2(1f, 1f);
                }
                else
                {
                    DecRect.anchorMin = new Vector2(1f, 0f);
                    DecRect.anchorMax = new Vector2(1f, 1f);

                    IncRect.anchorMin = new Vector2(1f, 0f);
                    IncRect.anchorMax = new Vector2(1f, 1f);
                }
                break;
            case Slider.Direction.RightToLeft:
                if (_isBarDecrease)
                {
                    IncRect.anchorMin = new Vector2(0f, 0f);
                    IncRect.anchorMax = new Vector2(0f, 1f);
                }
                else
                {
                    DecRect.anchorMin = new Vector2(0f, 0f);
                    DecRect.anchorMax = new Vector2(0f, 1f);

                    IncRect.anchorMin = new Vector2(0f, 0f);
                    IncRect.anchorMax = new Vector2(0f, 1f);
                }
                break;
            case Slider.Direction.BottomToTop:
                if (_isBarDecrease)
                {
                    IncRect.anchorMin = new Vector2(0f, 1f);
                    IncRect.anchorMax = new Vector2(1f, 1f);
                }
                else
                {
                    DecRect.anchorMin = new Vector2(0f, 1f);
                    DecRect.anchorMax = new Vector2(1f, 1f);
                }
                break;
            case Slider.Direction.TopToBottom:
                if (_isBarDecrease)
                {
                    IncRect.anchorMin = new Vector2(0f, 0f);
                    IncRect.anchorMax = new Vector2(1f, 0f);
                }
                else
                {
                    DecRect.anchorMin = new Vector2(0f, 0f);
                    DecRect.anchorMax = new Vector2(1f, 0f);
                }
                break;
        }
    }
}