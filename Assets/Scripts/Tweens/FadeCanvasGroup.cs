using System;
using UnityEngine;

namespace Michael
{
    [RequireComponent(typeof(CanvasGroup))]
    public class FadeCanvasGroup : IUserInterface
    {
        public Action<bool> OnFadeComplete;
        [SerializeField] float tweenTime = 0.5f;
        [SerializeField] bool showOnStart = false;
        CanvasGroup canvasGroup;
        void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
        void Start()
        {
            float targetAlpha = showOnStart ? 1 : 0;
            canvasGroup.alpha = targetAlpha;
        }
        public override void Hide()
        {
            Toggle(false);
        }

        public override void Show()
        {
            Toggle(true);
        }

        public override void Toggle()
        {
            Toggle(canvasGroup.alpha == 0);
        }

        public override void Toggle(bool show)
        {
            float targetAlpha = show ? 1 : 0;
            LeanTween.alphaCanvas(canvasGroup, targetAlpha, tweenTime)
                .setEaseInOutSine()
                .setOnComplete(() => OnFadeComplete?.Invoke(show));
        }
    }
}
