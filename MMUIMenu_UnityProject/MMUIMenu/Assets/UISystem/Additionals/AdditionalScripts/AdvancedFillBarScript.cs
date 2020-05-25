using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdvancedFillBarScript : MonoBehaviour
{
    public Slider Slider;
    public Image IncreaseFill, DecreaseFill;
    public TextMeshProUGUI PercentageText;
    public RectTransform PlaceholderGrid;
    public UISpawnController SpawnController;

    [Range(0f,1f)]
    public float InitialValue;
    public float DecreaseSpeed;
    public int HealthPerCell;

    public bool ShowAsPercentage;
    public bool ShowGrids;
    public bool BarGradientColor;

    public Color DecreaseColor = new Color(207.0f/255.0f, 42.0f/255.0f, 39.0f/255.0f), IncreaseColor = new Color(0.0f/255.0f, 158.0f/255.0f, 15.0f/255.0f);
    public Color FullColor = Color.white, EmptyColor = Color.white;

    RectTransform _incRect, _decRect;
    IEnumerator _updateRoutine;
    float _targetValue;
    bool _isBarDecrease;
    List<Image> _gridObjs = new List<Image>();

    private void Awake()
    {
        _updateRoutine = null;
        _isBarDecrease = false;

        _decRect = DecreaseFill.GetComponent<RectTransform>();
        _incRect = IncreaseFill.GetComponent<RectTransform>();

        _targetValue = InitialValue;
        Slider.value = InitialValue;

        IncreaseFill.color = IncreaseColor;
        DecreaseFill.color = DecreaseColor;
    }

    private void Start()
    {
        TooglePercentage(ShowAsPercentage);

        UpdatePercentageText(InitialValue * 100.0f);
    }

    public void UpdateBar(int percentage, bool instant = false)
    {
        if (_updateRoutine != null)
            StopCoroutine(_updateRoutine);

        _targetValue = percentage / 100.0f;

        UpdatePercentageText(percentage);

        _isBarDecrease = Slider.value > _targetValue;

        if(!instant)
        {
            _updateRoutine = UpdateSliderValue();
            StartCoroutine(_updateRoutine);
        }
        else
            Slider.value = _targetValue;
    }

    public void TooglePercentage(bool isActive)
    {
        ShowAsPercentage = isActive;

        PercentageText.gameObject.SetActive(ShowAsPercentage);
    }

    public void ActivateGrids(int maxHealth)
    {
        if (!CheckIfGridDrawCond(maxHealth))
            return;

        ShowGrids = true;

        InitGridPivot();

        InitGrids(maxHealth);

        OrderGrids();
    }

    public void DeactivateGrids()
    {
        ShowGrids = false;

        if (_gridObjs.Count != 0)
        {
            for (int i = _gridObjs.Count - 1; i > 0; i--)
                Destroy(_gridObjs[i]);
        }
    }

    private void UpdatePercentageText(float percentage)
    {
        PercentageText.text = "%" + percentage.ToString();
    }

    private bool CheckIfGridDrawCond(int maxHealth)
    {
        if (maxHealth < HealthPerCell)
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

        PlaceholderGrid.SetPivotWithCounterAdjustPosition(gridPivot);
    }

    private void InitGrids(int maxHealth)
    {
        if (_gridObjs.Count != 0)
            DeactivateGrids();

        int totalCellCount = maxHealth / HealthPerCell;
        int gridLineCount = totalCellCount;
        _gridObjs = SpawnController.LoadSpawnables<Image>(gridLineCount, true);
    }

    private float CalculateCellWidth(int gridLineCount)
    {
        float areaWidth = Slider.fillRect.rect.width;
        float pureArea = areaWidth - (gridLineCount * PlaceholderGrid.rect.width);

        return pureArea / gridLineCount;
    }

    private void OrderGrids()
    {
        int sign = 1;
        if (Slider.direction == Slider.Direction.RightToLeft)
            sign = -1;

        float cellWidth = CalculateCellWidth(_gridObjs.Count);

        float offset = 0f;
        for(int i = 0; i < _gridObjs.Count; i++)
        {
            if (i != 0)
                offset = PlaceholderGrid.rect.width;

            var newPos = _gridObjs[i].transform.localPosition;
            newPos.x = sign * (((i + 1) * cellWidth) + (i * offset));

            ((RectTransform)_gridObjs[i].transform).anchoredPosition = newPos;
        }
    }

    IEnumerator UpdateSliderValue()
    {
        while (true)
        {
            Slider.value = Mathf.Lerp(Slider.value, _targetValue, Time.deltaTime * DecreaseSpeed);

            SetIncDecBarAnchors();

            yield return null;
        }
    }

    void SetIncDecBarAnchors()
    {
        Vector2 anchorMax, anchorMin;

        ResetIncDecBarAnchor();

        if (_isBarDecrease)
        {
            anchorMax = _decRect.anchorMax;
            anchorMin = _decRect.anchorMin;
        }
        else
        {
            anchorMax = _incRect.anchorMax;
            anchorMin = _incRect.anchorMin;
        }

        switch (Slider.direction)
        {
            case Slider.Direction.LeftToRight:
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
            case Slider.Direction.BottomToTop:
            case Slider.Direction.TopToBottom:
                Debug.LogError("This state is not working yet, please use LeftToRight direction on slider");
                break;
        }

        if(_isBarDecrease)
        {
            _decRect.anchorMax = anchorMax;
            _decRect.anchorMin = anchorMin;
        }
        else
        {
            _incRect.anchorMax = anchorMax;
            _incRect.anchorMin = anchorMin;
        }
    }

    private void ResetIncDecBarAnchor()
    {
        switch (Slider.direction)
        {
            case Slider.Direction.LeftToRight:
                if (_isBarDecrease)
                {
                    _incRect.anchorMin = new Vector2(1f, 0f);
                    _incRect.anchorMax = new Vector2(1f, 1f);
                }
                else
                {
                    _decRect.anchorMin = new Vector2(1f, 0f);
                    _decRect.anchorMax = new Vector2(1f, 1f);
                }
                break;
            case Slider.Direction.RightToLeft:
                if (_isBarDecrease)
                {
                    _incRect.anchorMin = new Vector2(0f, 0f);
                    _incRect.anchorMax = new Vector2(0f, 1f);
                }
                else
                {
                    _decRect.anchorMin = new Vector2(0f, 0f);
                    _decRect.anchorMax = new Vector2(0f, 1f);
                }
                break;
            case Slider.Direction.BottomToTop:
            case Slider.Direction.TopToBottom:
                Debug.LogError("This state is not working yet, please use LeftToRight direction on slider");
                break;
        }
    }
}
