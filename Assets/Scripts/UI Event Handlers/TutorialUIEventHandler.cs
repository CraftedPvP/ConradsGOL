using TMPro;
using UnityEngine;

namespace Michael
{
    [CreateAssetMenu(fileName = "Tutorial UI", menuName = "Michael/UI Event Handler/Tutorial UI")]
    public class TutorialUIEventHandler : UIEventHandler
    {
        public void Show()
        {
            TutorialManager.Instance.Show();
        }
        public void Hide()
        {
            TutorialManager.Instance.Hide();
        }
    }
}
