using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestDragAndDropScript : MonoBehaviour
{
    public GameObject TargetCube;
    public RectTransform ImageTransform;
    public Canvas RenderCanvas;
    public bool ProjectToLocal;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
            ImageTransform.position = UISystemUtilities.ScreenToCanvasPos(ImageTransform, Input.mousePosition, Camera.main, RenderCanvas, ProjectToLocal);

        if (Input.GetKey(KeyCode.Space))
            ImageTransform.position = UISystemUtilities.WorldPosToCanvasPos(ImageTransform, TargetCube.transform.position, Camera.main, RenderCanvas, ProjectToLocal);
    }
}
