namespace Michael
{
    public interface IUserInterface
    {
        bool ShowOnStart { get; }
        bool IsAnimated { get; }
        void Show();
        void Hide();
        void Toggle();
        void Toggle(bool show);
    }
}
