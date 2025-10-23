using UnityEngine;

namespace Michael
{
    public class GameCanvasUITween : Singleton<GameCanvasUITween>, IUserInterface
    {
        [Range(.1f, .5f)]
        [SerializeField] float tweenTime = 0.35f;
        [SerializeField] float xOffset = 100f;
        CanvasGroup canvasGroup;

        [SerializeField] bool showOnStart = false;
        public bool ShowOnStart => showOnStart;

        [SerializeField] bool isAnimated = true;
        public bool IsAnimated => isAnimated;
        RectTransform rectTransform;
        public override void Awake()
        {
            base.Awake();
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
        }
        void Start()
        {
            bool tempIsAnimated = isAnimated;
            isAnimated = false;
            if (ShowOnStart) Show();
            else Hide();
            isAnimated = tempIsAnimated;
            LeanTween.value(gameObject, 0, 1, tweenTime);
        }

        public void Hide() => Toggle(false);
        public void Show() => Toggle(true);
        public void Toggle() => Toggle(canvasGroup.alpha == 0);
        public void Toggle(bool show)
        {
            if (!IsAnimated)
            {
                canvasGroup.alpha = show ? 1 : 0;
                return;
            }
            LeanTween.alphaCanvas(canvasGroup, show ? 1 : 0, tweenTime).setEaseInOutSine();
            
            Vector2 target = new Vector2(show ? xOffset : -xOffset, rectTransform.anchoredPosition.y);
            LeanTween.value(gameObject, rectTransform.anchoredPosition, target, tweenTime)
                .setOnUpdate((Vector2 val) => rectTransform.anchoredPosition = val).setEaseInOutSine();
        }

    }
}
