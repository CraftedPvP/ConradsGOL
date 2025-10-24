using UnityEngine;

namespace Michael {
    [CreateAssetMenu(fileName = "Game Settings", menuName = "Michael/Game Settings", order = 1)]
    public class GameSettings : ScriptableObject
    {
        // [Header("Map")]
        // [Tooltip("Starting map size (width, height)")]
        // [SerializeField] Vector2Int mapSize = new Vector2Int(10, 10);
        // public Vector2Int MapSize => mapSize;

        [Header("Tutorial")]
        [SerializeField] string tutorialKey = "HasCompletedTutorial";
        public string TutorialKey => tutorialKey;

        [Header("Lifecycle")]
        [Tooltip("Time in seconds before a living cell changes its state")]
        [SerializeField] float livingLife = 0.5f;
        public float LivingLife => livingLife;

        [Tooltip("Time in seconds a cell needs to transition to its new state")]
        [SerializeField] float transitionTime = .3f;
        public float TransitionTime => transitionTime;

        [Header("Cell")]
        [SerializeField] Cell cellPrefab;
        public Cell CellPrefab => cellPrefab;

        [Range(1f, 3f)]
        public float CellSize = 1f;

        public Color AliveColor = Color.yellow;
        public Color DeadColor = Color.gray;

        [Range(50,1000)]
        public int MinCells = 250;
        [Range(250,3000)]
        public int MaxCells = 2000;

        [SerializeField] LayerMask cellLayerMask;
        public LayerMask CellLayerMask => cellLayerMask;

        [Range(1, 2f)]
        [SerializeField] float raycastPadding = 1.25f;
        public float RaycastPadding => raycastPadding;

        [Header("Cell Actions")]
        [Tooltip("Action to set cell's life state randomly")]
        [SerializeField] ICellAction randomLifeChance;
        public ICellAction RandomLifeChance => randomLifeChance;

        [Tooltip("Action to update state of the cell")]
        [SerializeField] ICellAction changeState;
        public ICellAction ChangeState => changeState;
        
        [Tooltip("Action to check neighbors of the cell")]
        [SerializeField] ICellAction checkNeighbors;
        public ICellAction CheckNeighbors => checkNeighbors;

        [Tooltip("Action to set cell's life state randomly")]
        [SerializeField] ICellAction transitionState;
        public ICellAction TransitionState => transitionState;

        [Tooltip("Action to check if cell can expand to neighbors")]
        [SerializeField] ICellAction expand;
        public ICellAction Expand => expand;

        void OnValidate()
        {
            if (MinCells > MaxCells)
                MinCells = MaxCells;
        }
    }
}