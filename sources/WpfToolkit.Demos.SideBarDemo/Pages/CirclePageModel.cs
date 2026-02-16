namespace DustInTheWind.WpfToolkit.Demos.SideBarDemo.Pages;

public class CirclePageModel : ViewModelBase
{
    private readonly Timer timer;

    public string Text
    {
        get => field;
        private set
        {
            if (field == value)
                return;

            field = value;
            OnPropertyChanged();
        }
    }

    public CirclePageModel()
    {
        timer = new(HandleTimerTick, null, 1000, 1000);
    }

    private void HandleTimerTick(object state)
    {
        Initialize(() =>
        {
            Text = "Circle: " + DateTime.Now.ToString("hh:mm:ss");
        });
    }
}
