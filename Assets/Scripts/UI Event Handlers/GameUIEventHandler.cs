using UnityEngine;

namespace Michael
{
    [CreateAssetMenu(fileName = "Game UI", menuName = "Michael/UI Event Handler/Game UI")]
    public class GameUIEventHandler : UIEventHandler
    {
        public void PauseGameSpeed()
        {
            GameManager.Instance.PauseGameSpeed();
        }
        public void ResetGameSpeed()
        {
            GameManager.Instance.ResetGameSpeed();
        }
        public void SetGameSpeed(float speed)
        {
            // this returns a clamped float. just read it if we have a text display to show current speed
            GameManager.Instance.SetGameSpeed(speed);
        }
        public void StopSimulation()
        {
            GameManager.Instance.StopSimulation();
        }
        public void StartSimulation()
        {
            GameManager.Instance.StartSimulation();
        }
    }
}
