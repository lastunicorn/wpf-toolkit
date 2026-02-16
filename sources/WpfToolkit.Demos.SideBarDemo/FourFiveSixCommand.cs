using System.Windows;
using System.Windows.Input;

namespace DustInTheWind.WpfToolkit.Demos.SideBarDemo;

internal class FourFiveSixCommand : ICommand
{
    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
        return true;
    }

    public void Execute(object parameter)
    {
        MessageBox.Show("Button 456");
    }
}