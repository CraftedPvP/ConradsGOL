namespace Michael
{
    public class GameSpeedSliderToText : UISliderToText
    {
        void Start()
        {
            GameManager.Instance.OnGameSpeedChanged += UpdateText;
        }
    }
}
