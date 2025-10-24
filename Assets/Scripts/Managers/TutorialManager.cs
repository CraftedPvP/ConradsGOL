using UnityEngine;

namespace Michael
{
    public class TutorialManager : Singleton<TutorialManager>
    {
        string tutorialKey => GameManager.Instance.GameSettings.TutorialKey;
        [SerializeField] GameObject tutorialPanel;
        [SerializeField] FadeCanvasGroup canvasGroup;
        
        void Start()
        {
            canvasGroup.OnFadeComplete += OnCompleteTween;

            // first time playing the game
            if (!PlayerPrefs.HasKey(tutorialKey)) Show();
            else Hide();
        }
        public void Show()
        {
            gameObject.SetActive(true);
            tutorialPanel.SetActive(true);
            canvasGroup.Show();
        }
        public void Hide()
        {
            canvasGroup.Hide();
        }
        void OnCompleteTween(bool shown)
        {
            if (shown) return;
            tutorialPanel.SetActive(false);
            PlayerPrefs.SetInt(tutorialKey, 1); // mark tutorial as completed
            gameObject.SetActive(false);
        }
    }
}
