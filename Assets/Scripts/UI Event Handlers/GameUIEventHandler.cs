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
        public void Pause()
        {
            GameManager.Instance.Pause();
        }
        public void Play()
        {
            GameManager.Instance.Play();
        }
        public void SetGameSpeed(float speed)
        {
            // this returns a clamped float. just read it if we have a text display to show current speed
            GameManager.Instance.SetGameSpeed(speed);
        }
    }
}
