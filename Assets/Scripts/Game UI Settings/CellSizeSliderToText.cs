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
        { // dev note: the cell size must be a whole integer. otherwise, the grid calculations get messed up.
            GameManager.Instance.GameSettings.CellSize = value;
        }
        
        public void SetCellSize(float newSize)
        {
            OnValueChanged?.Invoke(newSize);
        }
    }
}
