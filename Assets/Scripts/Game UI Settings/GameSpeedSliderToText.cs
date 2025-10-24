namespace Michael
{
    public class GameSpeedSliderToText : UISliderToText
    {
        protected override void Start()
        {
            base.Start();
            GameManager.Instance.OnGameSpeedChanged += UpdateText;
        }
    }
}
