using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace DustInTheWind.WpfTools.CustomControls;

public class SideBarNavigator : TabControl
{
    #region Buttons DependencyProperty

    public static readonly DependencyProperty ButtonsProperty = DependencyProperty.Register(
        nameof(Buttons),
        typeof(ObservableCollection<Button>),
        typeof(SideBarNavigator),
        new PropertyMetadata(new ObservableCollection<Button>())
    );

    public ObservableCollection<Button> Buttons
    {
        get => (ObservableCollection<Button>)GetValue(ButtonsProperty);
        set => SetValue(ButtonsProperty, value);
    }

    #endregion

    static SideBarNavigator()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(SideBarNavigator), new FrameworkPropertyMetadata(typeof(SideBarNavigator)));
    }
}