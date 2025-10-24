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

        void OnDisable()
        {
            // turn it off when Game UI is closed
            flexibleColorPicker.gameObject.SetActive(false);
        }
        void Start()
        {
            // fixes a bug where the image's color changes upon first click
            flexibleColorPicker.startingColor = GameManager.Instance.GameSettings.AliveColor;
            // 2nd set is to set the color from our settings
            flexibleColorPicker.color = flexibleColorPicker.startingColor;
            flexibleColorPicker.onColorChange.AddListener(OnColorChanged_Event);
            Invoke(nameof(CloseColorPicker),0.2f); // allows for initialization

            image.color = flexibleColorPicker.color;
        }
        void CloseColorPicker()
        {
            flexibleColorPicker.gameObject.SetActive(false);
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