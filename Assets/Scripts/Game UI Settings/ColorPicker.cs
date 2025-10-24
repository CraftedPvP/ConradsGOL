using System;
using UnityEngine;
using UnityEngine.UI;

namespace Michael
{
    public class ColorPicker : MonoBehaviour
    {
        /// <summary>
        /// only raised when the color is changed during <see cref="GameState.Idle"/> state
        /// </summary>
        public static Action<Color> OnColorChanged;
        [SerializeField] FlexibleColorPicker flexibleColorPicker;
        [SerializeField] Image image;
        void Start()
        {
            flexibleColorPicker.onColorChange.AddListener(OnColorChanged_Event);
            flexibleColorPicker.gameObject.SetActive(false);
            
            flexibleColorPicker.color = GameManager.Instance.GameSettings.AliveColor;
            image.color = flexibleColorPicker.color;
        }

        void OnColorChanged_Event(Color color)
        {
            GameManager.Instance.GameSettings.AliveColor = color;
            image.color = color;

            // only raise the event during idle state
            if (GameManager.Instance.IsGameRunning) return;
            OnColorChanged?.Invoke(color);
        }
    }
}