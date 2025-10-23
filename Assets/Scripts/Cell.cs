using System;
using UnityEngine;

namespace Michael
{
    [SelectionBase]
    public class Cell : MonoBehaviour
    {
        [SerializeField] bool isAlive;
        public bool IsAlive => isAlive;
        /// <summary>
        /// The state the cell will transition to in the next generation
        /// </summary>
        public bool FutureIsAlive { get; set; }
        
        public int LiveNeighborCount
        {
            get
            {
                int liveCount = 0;
                foreach (var neighbor in Neighbors)
                {
                    if (neighbor != null && neighbor.IsAlive){
                        liveCount++;
                        // Debug.Log($"Neighbor at {neighbor.name} is alive.",neighbor);
                    }
                }
                return liveCount;
            }
        }

        public int DeadNeighborCount
        {
            get
            {
                int deadCount = 0;
                foreach (var neighbor in Neighbors)
                {
                    if (neighbor == null || !neighbor.IsAlive) deadCount++;
                }
                return deadCount;
            }
        }

        // the collider will remain on even if the cell is dead to allow for neighbor detection
        Collider2D cellCollider;
        SpriteRenderer spriteRenderer;
        // we don't need the animator but we'll keep it in-case we want to add more complex animations later
        Animator animator;
        /// <summary>
        /// this contains references to the neighboring cells including nulls for out of bounds neighbors.
        /// we want to keep track of nulls as we want to be able to extend the map based from the initial grid
        /// </summary>
        public Cell[] Neighbors = new Cell[8];
        public Collider2D Collider => cellCollider;
        GameObject spriteObject => transform.GetChild(0).gameObject;

        void Awake()
        {
            spriteRenderer = spriteObject.GetComponent<SpriteRenderer>();
            cellCollider = GetComponent<Collider2D>();
            animator = spriteObject.GetComponent<Animator>();
        }
        void Start()
        {
            ColorPicker.OnColorChanged += OnColorChanged;
            spriteRenderer.color = isAlive ? GameManager.Instance.GameSettings.AliveColor : GameManager.Instance.GameSettings.DeadColor;
            spriteObject.transform.localScale = Vector3.zero;
        }
        void OnDestroy()
        {
            ColorPicker.OnColorChanged -= OnColorChanged;
        }
        void OnColorChanged(Color color) => spriteRenderer.color = color;

        void OnDrawGizmosSelected()
        {
            for (int i = 0; i < CheckNeighborsCellAction.NeighborDirections.Length; i++)
            {
                Gizmos.color = Color.yellow;
                Vector2 neighborDir = CheckNeighborsCellAction.NeighborDirections[i];
                Gizmos.DrawRay(transform.position, neighborDir * GameManager.Instance.GameSettings.CellSize);
                
                if(Neighbors.Length <= i) continue;
                Cell neighbor = Neighbors[i];
                if (neighbor == null) continue;
                Gizmos.color = neighbor.IsAlive ? Color.green : Color.red;
                Vector3 offset = neighborDir * (GameManager.Instance.GameSettings.CellSize / 2);
                Vector2 direction = (neighbor.transform.position - transform.position).normalized;
                Gizmos.DrawRay(transform.position + offset, direction * GameManager.Instance.GameSettings.CellSize);
            }
        }
        public void TransitionState() => animator.SetTrigger($"To{(FutureIsAlive ? "Life" : "Death")}");
        /// <summary>
        /// called from <see cref="BecomeAlive"/> and <see cref="BecomeDead"/>
        /// </summary>
        public void TweenBasedOnState()
        {
            if (isAlive == FutureIsAlive) return; // no state change
            isAlive = FutureIsAlive;
            // Debug.Log($"Cell at {name} transitioning to {(isAlive ? "Alive" : "Dead")}");
            if (isAlive) TweenToLife();
            else TweenToDeath();
        }
        void TweenToLife()
        {
            Vector3 targetScale = Vector3.one * GameManager.Instance.GameSettings.CellSize;

            // skip tween if already alive
            if (spriteObject.transform.localScale == targetScale) return;

            // prevent tween overlap
            LeanTween.cancel(gameObject);
            LeanTween.cancel(spriteObject);

            // Animate color from deadColor to aliveColor
            Color deadColor = GameManager.Instance.GameSettings.DeadColor;
            Color aliveColor = GameManager.Instance.GameSettings.AliveColor;
            LeanTween.value(gameObject, ChangeColor, deadColor, aliveColor, GameManager.Instance.GameSettings.TransitionTime);

            // Animate scale from 0 to CellSize
            LeanTween.scale(spriteObject, targetScale, GameManager.Instance.GameSettings.TransitionTime);
        }
        void TweenToDeath()
        {
            // don't tween if already dead
            if (spriteObject.transform.localScale == Vector3.zero) return;

            // prevent tween overlap
            LeanTween.cancel(gameObject);
            LeanTween.cancel(spriteObject);

            // Animate color from aliveColor to deadColor
            Color deadColor = GameManager.Instance.GameSettings.DeadColor;
            Color aliveColor = GameManager.Instance.GameSettings.AliveColor;
            LeanTween.value(gameObject, ChangeColor, aliveColor, deadColor, GameManager.Instance.GameSettings.TransitionTime);

            // Animate scale from CellSize to 0
            LeanTween.scale(spriteObject, Vector3.zero, GameManager.Instance.GameSettings.TransitionTime)
                .setOnComplete(OnDeath);
        }
        void ChangeColor(Color targetColor) => spriteRenderer.color = targetColor;
        void OnDeath() {
            CellSpawner.Instance.Return(this);
            Map.Instance.OnCellDeath?.Invoke(this);
        }
    }
}