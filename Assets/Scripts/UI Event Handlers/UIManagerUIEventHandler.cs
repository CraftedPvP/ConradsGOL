using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Michael
{
    [CreateAssetMenu(fileName = "UI Manager", menuName = "Michael/UI Event Handler/UI Manager")]
    public class UIManagerUIEventHandler : UIEventHandler
    {
        public void PlayClickSound()
        {
            UIManager.Instance.PlayClickSound();
        }
    }
}
