namespace Michael
{
    public class MaxCellCountSliderToText : UISliderToText
    {
        protected override void Start()
        {
            base.Start();
            slider.minValue = GameManager.Instance.GameSettings.MinCells;
            slider.maxValue = GameManager.Instance.GameSettings.MaxCells;
            slider.value = GameManager.Instance.GameSettings.MinCells;
            slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        private void OnSliderValueChanged(float value)
        {
            GameManager.Instance.GameSettings.MaxCells = (int)value;
        }
    }
}
