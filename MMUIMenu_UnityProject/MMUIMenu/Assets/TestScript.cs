using UnityEngine;

public class TestScript : MonoBehaviour
{
    public Canvas CanvasToRender;
    public Transform ObjToFollow;

    RectTransform _rectTrans;

    private void Awake()
    {
        _rectTrans = GetComponent<RectTransform>();
    }

    private void Update()
    {
        Vector2 canvasPos = MMUISystemUtilities.WorldPosToCanvasPos(_rectTrans, ObjToFollow.transform.position, Camera.main, CanvasToRender);

        _rectTrans.anchoredPosition = canvasPos;
    }
}
