using UnityEngine;

namespace Michael
{
    [CreateAssetMenu(fileName = "Camera Settings", menuName = "Michael/Camera Settings", order = 2)]
    public class CameraSettings : ScriptableObject
    {
        [Header("Zoom")]
        [SerializeField] float zoomSpeed = 5f;
        public float ZoomSpeed => zoomSpeed;

        [SerializeField] Vector2 zoomLimits = new Vector2(2f, 20f);
        public Vector2 ZoomLimits => zoomLimits;

        [SerializeField] float zoomSmoothing = 0.1f;
        public float ZoomSmoothing => zoomSmoothing;

        [Header("Pan")]
        [SerializeField] float panSpeed = 5f;
        public float PanSpeed => panSpeed;

        [SerializeField] float panSmoothing = 0.1f;
        public float PanSmoothing => panSmoothing;

        void OnValidate()
        {
            zoomLimits.x = Mathf.Max(0.1f, zoomLimits.x);
            zoomLimits.y = Mathf.Max(zoomLimits.x, zoomLimits.y);
        }
    }
}