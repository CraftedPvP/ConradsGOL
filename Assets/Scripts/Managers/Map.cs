using System;
using System.Collections.Generic;
using UnityEngine;

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

        void Start()
        {
            ResetMap();
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
        }
        public void SpawnCellsAtChance() => ProcessMap(gameSettings.RandomLifeChance);
        public void CheckNeighbors() => ProcessMap(gameSettings.CheckNeighbors);
        public void UpdateCellState() => ProcessMap(gameSettings.UpdateState);
        public void TransitionCells() => ProcessMap(gameSettings.TransitionState);
        /// <summary>
        /// only meant to be called from <see cref="GameManager.ResetMap"/>
        /// </summary>
        public void ResetMap()
        {
            for (int i = 0; i < cells.Count; i++)
                CellSpawner.Instance.Return(cells[i]);
            cells.Clear();
            SpawnCells();
            OnMapReset?.Invoke();
        }
    }
}
