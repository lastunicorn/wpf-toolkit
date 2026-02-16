namespace DustInTheWind.WpfToolkit.Demos.SideBarDemo.Pages;

public class TrianglePageModel : ViewModelBase
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

    public TrianglePageModel()
    {
        timer = new(HandleTimerTick, null, 1000, 1000);
    }

    private void HandleTimerTick(object state)
    {
        Initialize(() =>
        {
            Text = "Triangle: " + DateTime.Now.ToString("hh:mm:ss");
        });
    }
}
