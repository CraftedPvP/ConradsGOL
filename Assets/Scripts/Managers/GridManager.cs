using UnityEngine;

namespace Michael
{
    /// <summary>
    /// Renders a grid overlay in the scene view and game view based on the camera's position and zoom level.
    /// </summary>
    public class GridManager : MonoBehaviour
    {
        [SerializeField] Color gridColor = new Color(1f, 1f, 1f, 0.2f);
        [SerializeField] Material lineMaterial;
        Camera mainCamera;

        void Awake()
        {
            if (lineMaterial == null)
            {
                // Create a simple colored material if not assigned
                Shader shader = Shader.Find("Hidden/Internal-Colored");
                lineMaterial = new Material(shader)
                {
                    hideFlags = HideFlags.HideAndDontSave
                };
                // Enable alpha blending
                lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
                lineMaterial.SetInt("_ZWrite", 0);
            }
        }
        void Start(){
            mainCamera = Camera.main;
        }

        void OnRenderObject()
        {
            if (mainCamera == null) mainCamera = Camera.main;
            if (mainCamera == null || GameManager.Instance == null || GameManager.Instance.GameSettings == null) return;

            float cellSize = GameManager.Instance.GameSettings.CellSize;

            // Get camera bounds in world space
            float camHeight = 2f * mainCamera.orthographicSize;
            float camWidth = camHeight * mainCamera.aspect;
            Vector3 camPos = mainCamera.transform.position;

            float left = camPos.x - camWidth / 2f;
            float right = camPos.x + camWidth / 2f;
            float bottom = camPos.y - camHeight / 2f;
            float top = camPos.y + camHeight / 2f;

            // Snap grid origin to nearest grid intersection
            float startX = Mathf.Floor(left / cellSize) * cellSize;
            float endX = Mathf.Ceil(right / cellSize) * cellSize;
            float startY = Mathf.Floor(bottom / cellSize) * cellSize;
            float endY = Mathf.Ceil(top / cellSize) * cellSize;

            lineMaterial.SetPass(0);
            GL.PushMatrix();
            // Use the camera's projection matrix so lines appear in the correct place
            GL.LoadProjectionMatrix(mainCamera.projectionMatrix);
            GL.modelview = mainCamera.worldToCameraMatrix;
            GL.Begin(GL.LINES);
            GL.Color(gridColor);

            float z = mainCamera.nearClipPlane + 0.01f; // Draw just in front of near plane

            // Draw vertical lines
            for (float x = startX; x <= endX; x += cellSize)
            {
                GL.Vertex3(x, startY, z);
                GL.Vertex3(x, endY, z);
            }

            // Draw horizontal lines
            for (float y = startY; y <= endY; y += cellSize)
            {
                GL.Vertex3(startX, y, z);
                GL.Vertex3(endX, y, z);
            }

            GL.End();
            GL.PopMatrix();
        }

        // Optionally, draw a thicker grid every gridSize cells
        void OnDrawGizmosSelected()
        {
            if (mainCamera == null) mainCamera = Camera.main;
            if (mainCamera == null || GameManager.Instance == null || GameManager.Instance.GameSettings == null) return;

            float cellSize = GameManager.Instance.GameSettings.CellSize;

            float camHeight = 2f * mainCamera.orthographicSize;
            float camWidth = camHeight * mainCamera.aspect;
            Vector3 camPos = mainCamera.transform.position;

            float left = camPos.x - camWidth / 2f;
            float right = camPos.x + camWidth / 2f;
            float bottom = camPos.y - camHeight / 2f;
            float top = camPos.y + camHeight / 2f;

            float startX = Mathf.Floor(left / cellSize) * cellSize;
            float endX = Mathf.Ceil(right / cellSize) * cellSize;
            float startY = Mathf.Floor(bottom / cellSize) * cellSize;
            float endY = Mathf.Ceil(top / cellSize) * cellSize;

            Gizmos.color = Color.yellow;
            // Draw thicker grid lines every gridSize
            for (float x = startX; x <= endX; x += cellSize * cellSize)
            {
                Gizmos.DrawLine(new Vector3(x, startY, 0), new Vector3(x, endY, 0));
            }
            for (float y = startY; y <= endY; y += cellSize * cellSize)
            {
                Gizmos.DrawLine(new Vector3(startX, y, 0), new Vector3(endX, y, 0));
            }
        }
    }
}