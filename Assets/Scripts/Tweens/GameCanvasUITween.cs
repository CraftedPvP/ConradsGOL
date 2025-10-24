using UnityEngine;

namespace Michael
{
    /// <summary>
    /// Handles tweening for game canvas UI elements
    /// </summary>
    public class GameCanvasUITween : IUserInterface
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
        public void Awake()
        {
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

        public override void Hide() => Toggle(false);
        public override void Show() => Toggle(true);
        public override void Toggle() => Toggle(canvasGroup.alpha == 0);
        public override void Toggle(bool show)
        {
            if (!IsAnimated)
            {
                canvasGroup.alpha = show ? 1 : 0;
                rectTransform.anchoredPosition = new Vector2(show ? xOffset : -xOffset, rectTransform.anchoredPosition.y);
                return;
            }
            LeanTween.alphaCanvas(canvasGroup, show ? 1 : 0, tweenTime).setEaseInOutSine();
            
            Vector2 target = new Vector2(show ? xOffset : -xOffset, rectTransform.anchoredPosition.y);
            LeanTween.value(gameObject, rectTransform.anchoredPosition, target, tweenTime)
                .setOnUpdate((Vector2 val) => rectTransform.anchoredPosition = val).setEaseInOutSine();
        }

    }
}
