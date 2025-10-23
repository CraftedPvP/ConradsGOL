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
        public Cell[] Neighbors = new Cell[CheckNeighborsCellAction.NeighborDirections.Length];
        public Collider2D Collider => cellCollider;
        GameObject spriteObject => transform.GetChild(0).gameObject;

        // Tweening fields for manual interpolation
        struct TweenInfo {
            public Vector3 startScale, targetScale;
            public Color startColor, targetColor;
            public float timeElapsed, tweenDuration;
        }
        TweenInfo tweenInfo;
        void Awake()
        {
            spriteRenderer = spriteObject.GetComponent<SpriteRenderer>();
            cellCollider = GetComponent<Collider2D>();
            animator = spriteObject.GetComponent<Animator>();
        }
        void Start()
        {
            spriteRenderer.color = isAlive ? GameManager.Instance.GameSettings.AliveColor : GameManager.Instance.GameSettings.DeadColor;
            spriteObject.transform.localScale = Vector3.zero;
            enabled = false;
        }
        void Update()
        {
            AnimateOverTime();
        }
        public void ChangeColor(Color color) => spriteRenderer.color = color;

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
            enabled = true;
        }
        void SetTweenToAnimate(Vector3 targetScale, Color targetColor)
        {
            tweenInfo.startScale = spriteObject.transform.localScale;
            tweenInfo.targetScale = targetScale;
            tweenInfo.startColor = spriteRenderer.color;
            tweenInfo.targetColor = targetColor;
            tweenInfo.timeElapsed = 0f;
            tweenInfo.tweenDuration = GameManager.Instance.GameSettings.TransitionTime;
        }
        void AnimateOverTime(){
            // Interpolate
            tweenInfo.timeElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(tweenInfo.timeElapsed / tweenInfo.tweenDuration);
            spriteObject.transform.localScale = Vector3.Lerp(tweenInfo.startScale, tweenInfo.targetScale, t);
            spriteRenderer.color = Color.Lerp(tweenInfo.startColor, tweenInfo.targetColor, t);

            if (t >= 1f)
            {
                spriteObject.transform.localScale = tweenInfo.targetScale;
                spriteRenderer.color = tweenInfo.targetColor;
                enabled = false;
                if (!isAlive) OnDeath();
            }
        }
        void TweenToLife()
        {
            Vector3 targetScale = Vector3.one * GameManager.Instance.GameSettings.CellSize;
            Color aliveColor = GameManager.Instance.GameSettings.AliveColor;
            SetTweenToAnimate(targetScale, aliveColor);
        }
        void TweenToDeath()
        {
            Vector3 targetScale = Vector3.zero;
            Color deadColor = GameManager.Instance.GameSettings.DeadColor;
            SetTweenToAnimate(targetScale, deadColor);
        }
        void OnDeath() {
            CellSpawner.Instance.Return(this);
            Map.Instance.OnCellDeath?.Invoke(this);
        }
    }
}