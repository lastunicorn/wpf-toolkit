using System.Windows;

namespace DustInTheWind.WpfToolkit.Demos.SideBarDemo;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    override protected void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        MainWindow mainWindow = new()
        {
            DataContext = new MainViewModel()
        };

        mainWindow.Show();

        MainWindow = mainWindow;
    }
}

