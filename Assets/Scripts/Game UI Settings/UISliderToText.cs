using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Michael
{
    public class UISliderToText : MonoBehaviour
    {
        public Action<float> OnValueChanged;
        [SerializeField] TextMeshProUGUI text;
        [SerializeField] Slider slider;
        [SerializeField] string formatString = "{0}";
        void Start()
        {
            slider.onValueChanged.AddListener(UpdateText);
            UpdateText(slider.value);
        }

        protected void UpdateText(float value) => text.text = string.Format(formatString, value);
        public void ForceUpdate() => UpdateText(slider.value);
    }
}
