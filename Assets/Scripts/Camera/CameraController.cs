using System;
using UnityEngine;
using UnityEngine.InputSystem;

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
            PlayerController.Instance.Controls.Game.Zoom.performed += ProcessZoom;
        }
        void ProcessZoom(InputAction.CallbackContext context)
        {
            float zoomInput = context.ReadValue<Vector2>().y;
            targetOrthoSize -= zoomInput * cameraSettings.ZoomSpeed * Time.deltaTime;
            targetOrthoSize = Mathf.Clamp(targetOrthoSize, cameraSettings.ZoomLimits.x, cameraSettings.ZoomLimits.y);
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
            // Smooth Zoom
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetOrthoSize, cameraSettings.ZoomSmoothing * Time.deltaTime);

            // Smooth Pan
            Vector2 panInput = PlayerController.Instance.Controls.Game.Pan.ReadValue<Vector2>();
            targetPosition += new Vector3(panInput.x, panInput.y, 0f) * cameraSettings.PanSpeed * Time.deltaTime;
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, cameraSettings.PanSmoothing * Time.deltaTime);
        }
    }
}
