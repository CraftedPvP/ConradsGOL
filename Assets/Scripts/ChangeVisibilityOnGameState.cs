using UnityEngine;

namespace Michael
{
    public class ChangeVisibilityOnGameState : MonoBehaviour
    {
        [SerializeField] GameState activeState = GameState.Idle;
        void Start()
        {
            GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
            // set initial state
            OnGameStateChanged(GameManager.Instance.CurrentGameState);
        }

        void OnGameStateChanged(GameState state)
        {
            gameObject.SetActive(state == activeState);
        }
    }
}
