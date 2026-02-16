using System.Windows;

namespace DustInTheWind.Demos.SideBarDemo;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Button_Click_123(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Button 123");
    }

    private void Button_Click_456(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Button 456");
    }
}