using UnityEngine;

namespace MVVM
{
    [RequireComponent(typeof(Canvas))]
    public class UICameraFinder : MonoBehaviour
    {
        public RenderMode CanvasRenderMode;
        public float PlaneDistance;

        public bool ByTag;
        public bool ByLayer;

        public string Tag;
        public int Layer;

        private void Awake()
        {
            Canvas targetCanvas = GetComponent<Canvas>();
            targetCanvas.renderMode = CanvasRenderMode;
            targetCanvas.planeDistance = PlaneDistance;

            if (ByTag)
                targetCanvas.worldCamera = CheckByTag();
            else if (ByLayer)
                targetCanvas.worldCamera = CheckByLayer();
            else
                targetCanvas.worldCamera = CheckByBoth();
        }

        private Camera CheckByTag()
        {
            Camera[] allActiveCamColl = FindObjectsOfType<Camera>();

            for (int i = 0; i < allActiveCamColl.Length; i++)
            {
                if (allActiveCamColl[i].tag.Equals(Tag))
                    return allActiveCamColl[i];
            }

            return null;
        }

        private Camera CheckByLayer()
        {
            Camera[] allActiveCamColl = FindObjectsOfType<Camera>();

            for (int i = 0; i < allActiveCamColl.Length; i++)
            {
                if (allActiveCamColl[i].gameObject.layer.Equals(Layer))
                    return allActiveCamColl[i];
            }

            return null;
        }

        private Camera CheckByBoth()
        {
            Camera[] allActiveCamColl = FindObjectsOfType<Camera>();

            for (int i = 0; i < allActiveCamColl.Length; i++)
            {
                if (allActiveCamColl[i].tag.Equals(Tag)
                    && allActiveCamColl[i].gameObject.layer.Equals(Layer))
                    return allActiveCamColl[i];
            }

            return null;
        }
    }

}