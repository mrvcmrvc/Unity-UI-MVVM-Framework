using UnityEngine;
using UnityEngine.UI;

public class UIInputBlocker : MonoBehaviour
{
    [SerializeField] private bool _canBlockWorldInput;

    private Graphic[] _graphics;

    private void Awake()
    {
        GetAllChildGraphics();

        UpdateGraphics();
    }

    private void OnTransformChildrenChanged()
    {
        GetAllChildGraphics();

        UpdateGraphics();
    }

    public void SetInputBlocker(bool canBlock)
    {
        _canBlockWorldInput = canBlock;

        UpdateGraphics();
    }

    private void GetAllChildGraphics()
    {
        _graphics = GetComponentsInChildren<Graphic>();
    }

    private void UpdateGraphics()
    {
        for(int i = 0; i < _graphics.Length; i++)
            _graphics[i].raycastTarget = _canBlockWorldInput;
    }
}
