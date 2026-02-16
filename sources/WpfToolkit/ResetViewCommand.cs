using System.Windows.Input;

namespace DustInTheWind.WpfToolkit;

internal class ResetViewCommand : ICommand
{
    private readonly ZoomPanel zoomPanControl;

    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public ResetViewCommand(ZoomPanel zoomPanControl)
    {
        this.zoomPanControl = zoomPanControl ?? throw new ArgumentNullException(nameof(zoomPanControl));
    }

    public bool CanExecute(object parameter)
    {
        return true;
    }

    public void Execute(object parameter)
    {
        zoomPanControl.Reset();
    }
}
