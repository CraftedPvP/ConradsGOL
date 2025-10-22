using UnityEngine;

namespace Michael {
    [CreateAssetMenu(fileName = "Game Settings", menuName = "Michael/Game Settings", order = 1)]
    public class GameSettings : ScriptableObject
    {
        [Header("Map")]
        [Tooltip("Starting map size (width, height)")]
        [SerializeField] Vector2Int mapSize = new Vector2Int(10, 10);
        public Vector2Int MapSize => mapSize;

        [Header("Life")]
        [Tooltip("Time in seconds before a living cell changes its state")]
        [SerializeField] float livingLife = 0.5f;
        public float LivingLife => livingLife;

        [Tooltip("Time in seconds a cell needs to transition to its new state")]
        [SerializeField] float transitionTime = .3f;
        public float TransitionTime => transitionTime;

        [Header("Cell")]
        [SerializeField] Cell cellPrefab;
        public Cell CellPrefab => cellPrefab;

        [SerializeField] float cellSize = 1f;
        public float CellSize => cellSize;

        public Color AliveColor;
        public Color DeadColor;

        [SerializeField] int maxCells = 2000;
        public int MaxCells => maxCells;

        [Header("Cell Actions")]
        [Tooltip("Action to set cell's life state randomly")]
        [SerializeField] ICellAction randomLifeChance;
        public ICellAction RandomLifeChance => randomLifeChance;

        [Tooltip("Action to update state of the cell")]
        [SerializeField] ICellAction updateState;
        public ICellAction UpdateState => updateState;
        
        [Tooltip("Action to check neighbors of the cell")]
        [SerializeField] ICellAction checkNeighbors;
        public ICellAction CheckNeighbors => checkNeighbors;

        [Tooltip("Action to set cell's life state randomly")]
        [SerializeField] ICellAction transitionState;
        public ICellAction TransitionState => transitionState;
    }
}