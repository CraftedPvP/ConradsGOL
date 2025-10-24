using System;

namespace Michael
{
    public class CellSizeSliderToText : UISliderToText
    {
        protected override void Start()
        {
            base.Start();
            slider.value = GameManager.Instance.GameSettings.CellSize;
            slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        void OnSliderValueChanged(float value)
        {
            GameManager.Instance.GameSettings.CellSize = value;
        }
        
        public void SetCellSize(float newSize)
        {
            OnValueChanged?.Invoke(newSize);
        }
    }
}
