using UnityEngine;

namespace MVVM
{
    [ExecuteInEditMode, RequireComponent(typeof(Canvas))]
    public class CanvasLookAtCameraScript : MonoBehaviour
    {
        public bool LookToDir;

        private void Update()
        {
            transform.LookAt(Camera.main.transform);

            if (LookToDir)
            {
                Quaternion quaternion = transform.rotation;
                Quaternion newQuaternion = Quaternion.Euler(-1 * Camera.main.transform.localRotation.eulerAngles.x, 180f, quaternion.eulerAngles.z);
                transform.rotation = newQuaternion;
            }
        }

        private void OnEnable()
        {
            Canvas targetCanvas = GetComponent<Canvas>();

            if (!targetCanvas.renderMode.Equals(RenderMode.WorldSpace))
                Debug.LogWarning("The canvas on the " + gameObject.name + " must be set to world space rendering, to be able to use look at functionality!");
        }
    }
}