using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Michael {
    /// <summary>
    /// This manages the <see cref="Cell"/> through its lifecycle
    /// </summary>
    public class Map : Singleton<Map>
    {
        public static Action OnMapReset;

        [Header("General")]
        [SerializeField] GameSettings gameSettings;

        [Header("Game Elements")]
        [SerializeField] List<Cell> cells;
        public int CellsInMapCount => cells.Count;
        public int AliveCellsCount
        {
            get
            {
                int count = 0;
                for (int i = 0; i < cells.Count; i++)
                {
                    if (cells[i].IsAlive) count++;
                }
                return count;
            }
        }

        Camera mainCamera;

        void Start()
        {
            mainCamera = Camera.main;
            PlayerController.Instance.Controls.Game.Select.performed += OnPlayerSelectGrid;

            ResetMap();
        }

        void OnPlayerSelectGrid(InputAction.CallbackContext context)
        {
            if (GameManager.Instance.IsGameRunning) {
                Debug.LogWarning("Game is running. Cannot select spawn cell.");
                return;
            }

            // Get mouse position in world space
            Vector2 mouseScreenPos = PlayerController.Instance.Controls.Game.SelectPos.ReadValue<Vector2>();
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);

            // clamp to nearest grid position
            float cellSize = gameSettings.CellSize;
            Vector3 clampedPos = new Vector3(
                Mathf.Floor(mouseWorldPos.x / cellSize) * cellSize + cellSize / 2f,
                Mathf.Floor(mouseWorldPos.y / cellSize) * cellSize + cellSize / 2f,
                0f);

            // check if cell exists at that position
            Collider2D cellCollider = Physics2D.OverlapPoint(clampedPos, gameSettings.CellLayerMask);
            // if no cell exists, spawn a cell
            if (!cellCollider)
            {
                Cell newCell = CellSpawner.Instance.Get();
                newCell.transform.position = clampedPos;
                cells.Add(newCell);
                newCell.FutureIsAlive = true;
                gameSettings.TransitionState.Execute(newCell);
                return;
            }
            // if a cell exists, kill the cell
            Cell existingCell = cellCollider.GetComponent<Cell>();
            existingCell.FutureIsAlive = false;
            gameSettings.TransitionState.Execute(existingCell);
        }

        void ProcessMap(ICellAction action)
        {
            for (int i = 0; i < cells.Count; i++)
            {
                if (cells[i]) action.Execute(cells[i]);
            }
        }
        void SpawnCells()
        {
            for (int i = 0; i < gameSettings.MapSize.x * gameSettings.MapSize.y; i++)
            {
                Vector3 position = new Vector3(
                    (i % gameSettings.MapSize.x) * gameSettings.CellSize,
                    (i / gameSettings.MapSize.x) * gameSettings.CellSize,
                    0f);
                Cell newCell = CellSpawner.Instance.Get();
                newCell.transform.position = position;
                cells.Add(newCell);
            }
            SpawnCellsAtChance();
            TransitionCells();
            GameManager.Instance.CallUpdateStats();
        }
        public void SpawnCellsAtChance() => ProcessMap(gameSettings.RandomLifeChance);
        public void CheckNeighbors() => ProcessMap(gameSettings.CheckNeighbors);
        public void ChangeCellState() => ProcessMap(gameSettings.ChangeState);
        public void TransitionCells() => ProcessMap(gameSettings.TransitionState);
        /// <summary>
        /// only meant to be called from <see cref="GameManager.ResetMap"/>
        /// </summary>
        public void ResetMap()
        {
            ClearMap();
            SpawnCells();
            OnMapReset?.Invoke();
        }
        public void ClearMap()
        {
            for (int i = 0; i < cells.Count; i++)
                CellSpawner.Instance.Return(cells[i]);
            cells.Clear();
        }
    }
}
