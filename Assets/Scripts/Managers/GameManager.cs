using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Michael
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] GameSettings gameSettings;
        public GameSettings GameSettings => gameSettings;

        [SerializeField] float gameplayTime = 0f;
        bool hasTransitioned = true;

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
                Map.Instance.UpdateCellState();
                Map.Instance.TransitionCells();
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
    }
}
