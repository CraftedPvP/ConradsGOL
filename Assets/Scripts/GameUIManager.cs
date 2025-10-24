using UnityEngine;

namespace Michael
{
    public class GameUIManager : Singleton<GameUIManager>
    {
        [SerializeField] IUserInterface uiPanel;
        [SerializeField] CellSizeSliderToText CellSizeSliderToText;

        void Start()
        {
            ColorPicker.OnColorChanged += ChangeCellColor;
            CellSizeSliderToText.OnValueChanged += ChangeCellSize;
        }

        void OnDestroy()
        {
            ColorPicker.OnColorChanged -= ChangeCellColor;
            CellSizeSliderToText.OnValueChanged -= ChangeCellSize;
        }
        public void Toggle(bool show) => uiPanel.Toggle(show);

        void ChangeCellColor(Color color)
        {
            foreach (var cell in Map.Instance.Cells)
                cell.ChangeColor(color);
        }
        void ChangeCellSize(float newSize)
        {
            foreach (var cell in Map.Instance.Cells)
                cell.ChangeSize(newSize);
        }
    }
}
