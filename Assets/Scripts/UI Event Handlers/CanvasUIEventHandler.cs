using UnityEngine;

namespace Michael
{
    [CreateAssetMenu(fileName = "Canvas UI", menuName = "Michael/UI Event Handler/Canvas UI")]
    public class CanvasUIEventHandler : UIEventHandler
    {
        public void ToggleGameUIPanel(bool willShow)
        {
            GameUIManager.Instance.Toggle(willShow);
        }
    }
}
