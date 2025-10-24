using System;
using UnityEngine;

namespace Michael
{
    public enum GameState
    {
        Idle,
        Running
    }
    /// <summary>
    /// Manages the lifecycle of the simulation.
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        public Action<GameState> OnGameStateChanged;
        public Action<float> OnGameSpeedChanged;
        GameState _currentGameState = GameState.Idle;
        public GameState CurrentGameState {
            get => _currentGameState;
            private set { _currentGameState = value; OnGameStateChanged?.Invoke(value); }
        }
        
        [SerializeField] GameSettings gameSettings;
        public GameSettings GameSettings => gameSettings;

        [SerializeField] float gameplayTime = 0f;
        bool hasTransitioned = true;

        int generation = 0;
        public int Generation => generation;
        public bool IsGameRunning => CurrentGameState == GameState.Running;

        void Start()
        {
            // disable here so we can manually control when Update is called
            gameObject.SetActive(false);
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
                Map.Instance.ProcessLifecycle();
                IncrementGeneration();
                CallUpdateStats();
            }

            // reset for next lifecycle
            if (gameplayTime >= oneLifecycle)
            {
                gameplayTime = 0f;
                hasTransitioned = false;
            }
        }
        public void StartSimulation()
        {
            // reset values
            gameplayTime = 0f;
            generation = 0;
            hasTransitioned = false;
            
            // prepare map
            Map.Instance.TransitionCells();
            CallUpdateStats();
            
            // start simulation
            gameObject.SetActive(true);
            // toggle game ui on via event
            CurrentGameState = GameState.Running;
        }
        public void StopSimulation()
        {
            CurrentGameState = GameState.Idle;
            // prevent simulation from running
            gameObject.SetActive(false);

            // reset values
            gameplayTime = 0f;
            generation = 0;
            hasTransitioned = false;

            Map.Instance.ClearMap();
            CallUpdateStats();
        }
        #region Game Speed
        public void ResetGameSpeed() => SetGameSpeed(1f);
        public void PauseGameSpeed() => SetGameSpeed(0f);
        public float SetGameSpeed(float speed)
        {
            speed = Mathf.Clamp(speed, 0f, 3f);
            Time.timeScale = speed;
            OnGameSpeedChanged?.Invoke(Time.timeScale);
            return Time.timeScale;
        }
        #endregion
        #region Stats
        public void CallUpdateStats()
        {
            // allow Animator to change state first
            Invoke(nameof(UpdateStats), 0.1f);
            // if we don't delay the call, newly created living cell upon calling update stats will be 0
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
            Stats.Instance.OnStatUpdated?.Invoke(Stats.StatType.Generation, generation.ToString());
        }
        public void IncrementGeneration()
        {
            generation++;
            UpdateGenerationStat();
        }
        #endregion
    }
}
