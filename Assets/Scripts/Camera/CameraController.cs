using UnityEngine;

namespace Michael
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] CameraSettings cameraSettings;
        Camera mainCamera;
        float targetOrthoSize;
        Vector3 targetPosition;

        void Start()
        {
            mainCamera = Camera.main;
            targetOrthoSize = mainCamera.orthographicSize;
            targetPosition = mainCamera.transform.position;
            Map.OnMapReset += CenterToMap;
        }
        void OnDestroy()
        {
            Map.OnMapReset -= CenterToMap;            
        }
        public void CenterToMap()
        {
            Vector2 mapSize = new Vector2(
                GameManager.Instance.GameSettings.MapSize.x * GameManager.Instance.GameSettings.CellSize,
                GameManager.Instance.GameSettings.MapSize.y * GameManager.Instance.GameSettings.CellSize
            );
            // Assuming the center cell is at the center of the map
            Vector3 centerPosition = new Vector3(mapSize.x / 2f, mapSize.y / 2f, mainCamera.transform.position.z);
            targetPosition = centerPosition;
            mainCamera.transform.position = centerPosition;
        }

        void Update()
        {
            float zoom = Input.GetAxis("Mouse ScrollWheel");
            float panX = Input.GetAxis("Horizontal");
            float panY = Input.GetAxis("Vertical");

            // Smooth Zoom
            if (zoom != 0f)
            {
                targetOrthoSize -= zoom * cameraSettings.ZoomSpeed;
                targetOrthoSize = Mathf.Clamp(targetOrthoSize, cameraSettings.ZoomLimits.x, cameraSettings.ZoomLimits.y);
            }
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetOrthoSize, cameraSettings.ZoomSmoothing * Time.deltaTime);

            // Smooth Pan
            if (panX != 0f || panY != 0f)
            {
                Vector3 panMovement = new Vector3(panX, panY, 0f) * cameraSettings.PanSpeed * Time.deltaTime;
                targetPosition += panMovement;
            }
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, cameraSettings.PanSmoothing * Time.deltaTime);
        }
    }
}
