using System.Windows;
using System.Windows.Controls;

namespace DustInTheWind.WpfToolkit;

public class ShyControl : ContentControl
{
    static ShyControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ShyControl), new FrameworkPropertyMetadata(typeof(ShyControl)));
    }
}
