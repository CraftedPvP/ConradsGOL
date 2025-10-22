using UnityEngine;

namespace Michael
{
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
                    if (neighbor != null && neighbor.IsAlive) liveCount++;
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

        SpriteRenderer spriteRenderer;
        Collider2D cellCollider;
        public Cell[] Neighbors;

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            cellCollider = GetComponent<Collider2D>();
        }
        public void TweenBasedOnState()
        {
            isAlive = FutureIsAlive;

            if (isAlive) TweenToLife();
            else TweenToDeath();
        }
        void TweenToLife()
        {
            cellCollider.enabled = true;

            // prevent tween overlap
            LeanTween.cancel(gameObject);

            // Animate color from deadColor to aliveColor
            Color deadColor = GameManager.Instance.GameSettings.DeadColor;
            Color aliveColor = GameManager.Instance.GameSettings.AliveColor;
            LeanTween.value(gameObject, ChangeColor, deadColor, aliveColor, GameManager.Instance.GameSettings.TransitionTime);

            // Animate scale from 0 to 1
            transform.localScale = Vector3.zero;
            LeanTween.scale(gameObject, Vector3.one, GameManager.Instance.GameSettings.TransitionTime);
        }
        void TweenToDeath()
        {
            cellCollider.enabled = false;

            // prevent tween overlap
            LeanTween.cancel(gameObject);

            // Animate color from aliveColor to deadColor
            Color deadColor = GameManager.Instance.GameSettings.DeadColor;
            Color aliveColor = GameManager.Instance.GameSettings.AliveColor;
            LeanTween.value(gameObject, ChangeColor, aliveColor, deadColor, GameManager.Instance.GameSettings.TransitionTime);

            // Animate scale from 1 to 0
            LeanTween.scale(gameObject, Vector3.zero, GameManager.Instance.GameSettings.TransitionTime);
        }
        void ChangeColor(Color targetColor) => spriteRenderer.color = targetColor;
    }
}