using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdvancedFillBarScript : MonoBehaviour
{
    public Slider Slider;
    public Image IncreaseFill, DecreaseFill;
    public TextMeshProUGUI PercentageText;
    [Range(0f,1f)]
    public float InitialValue;
    public float DecreaseSpeed;
    public bool ShowAsPercentage;
    public bool BarGradientColor;
    public Color DecreaseColor = new Color(207.0f/255.0f, 42.0f/255.0f, 39.0f/255.0f), IncreaseColor = new Color(0.0f/255.0f, 158.0f/255.0f, 15.0f/255.0f);
    public Color FullColor = Color.white, EmptyColor = Color.white;

    RectTransform _incRect, _decRect;
    IEnumerator _updateRoutine;
    float _targetValue;
    bool _isBarDecrease;

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

        TooglePercentage(ShowAsPercentage);

        UpdatePercentageText(InitialValue * 100.0f);
    }

    //private void Update()
    //{
    //    if(Input.GetKeyUp(KeyCode.Keypad1))
    //        UpdateBar(100f);

    //    if (Input.GetKeyUp(KeyCode.Keypad2))
    //        UpdateBar(75f);

    //    if (Input.GetKeyUp(KeyCode.Keypad3))
    //        UpdateBar(50f);

    //    if (Input.GetKeyUp(KeyCode.Keypad4))
    //        UpdateBar(25f);

    //    if (Input.GetKeyUp(KeyCode.Keypad5))
    //        UpdateBar(0f);
    //}

    public void UpdateBar(int percentage)
    {
        if (_updateRoutine != null)
            StopCoroutine(_updateRoutine);

        _targetValue = percentage / 100.0f;

        UpdatePercentageText(percentage);

        _isBarDecrease = Slider.value > _targetValue;

        _updateRoutine = UpdateStaminaBar();
        StartCoroutine(_updateRoutine);
    }

    void UpdatePercentageText(float percentage)
    {
        PercentageText.text = "%" + percentage.ToString();
    }

    public void TooglePercentage(bool isActive)
    {
        ShowAsPercentage = isActive;

        PercentageText.gameObject.SetActive(ShowAsPercentage);
    }

    IEnumerator UpdateStaminaBar()
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

        if (_isBarDecrease)
        {
            _incRect.anchorMin = new Vector2(1f, 0f);
            _incRect.anchorMax = new Vector2(1f, 1f);

            anchorMax = _decRect.anchorMax;
            anchorMin = _decRect.anchorMin;
        }
        else
        {
            _decRect.anchorMin = new Vector2(1f, 0f);
            _decRect.anchorMax = new Vector2(1f, 1f);

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
}
