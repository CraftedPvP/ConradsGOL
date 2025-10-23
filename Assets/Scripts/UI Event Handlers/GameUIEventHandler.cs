using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Michael
{
    [CreateAssetMenu(fileName = "Game UI", menuName = "Michael/UI Event Handler/Game UI")]
    public class GameUIEventHandler : UIEventHandler
    {
        public void ResetMap()
        {
            GameManager.Instance.ResetMap();
        }
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
    }
}
