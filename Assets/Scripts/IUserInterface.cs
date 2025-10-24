using UnityEngine;

namespace Michael
{
    public abstract class IUserInterface : MonoBehaviour
    {
        bool ShowOnStart { get; }
        bool IsAnimated { get; }
        public abstract void Show();
        public abstract void Hide();
        public abstract void Toggle();
        public abstract void Toggle(bool show);
    }
}
