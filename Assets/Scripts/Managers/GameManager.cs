using UnityEngine;

namespace Michael
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] GameSettings gameSettings;
        public GameSettings GameSettings => gameSettings;

        [SerializeField] float gameplayTime = 0f;
        bool hasTransitioned = true;

        int generation = 0;
        public int Generation => generation;

        public override void Awake()
        {
            base.Awake();
        }
            
        void Update()
        {
            ProcessLifecycleTime();
        }
        void ProcessLifecycleTime()
        {
            gameplayTime += Time.deltaTime;
            float oneLifecycle = gameSettings.LivingLife + gameSettings.TransitionTime;

            // Living Phase
            if (gameplayTime < gameSettings.LivingLife) return;

            // transition phase. called once per lifecycle
            if (!hasTransitioned && gameplayTime < oneLifecycle)
            {
                hasTransitioned = true;
                Map.Instance.CheckNeighbors();
                Map.Instance.ChangeCellState();
                Map.Instance.TransitionCells();
                CallUpdateStats();
            }

            // reset for next lifecycle
            if (gameplayTime >= oneLifecycle)
            {
                gameplayTime = 0f;
                hasTransitioned = false;
            }
        }
        public void ResetMap()
        {
            gameplayTime = 0f;
            hasTransitioned = true;
            Map.Instance.ResetMap();
        }
        #region Game Speed
        public void ResetGameSpeed()
        {
            Time.timeScale = 1f;
        }

        public void PauseGameSpeed()
        {
            Time.timeScale = 0f;
        }
        public float SetGameSpeed(float speed)
        {
            speed = Mathf.Clamp(speed, 0f, 3f);
            Time.timeScale = speed;
            return Time.timeScale;
        }
        #endregion
        #region Stats
        public void CallUpdateStats()
        {
            // allow Animator to change state first
            Invoke(nameof(UpdateStats), 0.1f);
            // if we don't delay the call, alive cells on the first generation will be 0
        }
        void UpdateStats()
        {
            UpdateAliveCellStat();
            UpdateGenerationStat();
        }
        void UpdateAliveCellStat()
        {
            Stats.Instance.OnStatUpdated?.Invoke(Stats.StatType.AliveCells, Map.Instance.AliveCellsCount.ToString());
        }
        void UpdateGenerationStat()
        {
            generation++;
            Stats.Instance.OnStatUpdated?.Invoke(Stats.StatType.Generation, generation.ToString());
        }
        #endregion
    }
}
