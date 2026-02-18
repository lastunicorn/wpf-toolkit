using System.Windows;
using System.Windows.Input;

namespace DustInTheWind.WpfToolkit;

public static class WindowDragBehavior
{
    public static readonly DependencyProperty EnableDragProperty =
        DependencyProperty.RegisterAttached(
            "EnableDrag",
            typeof(bool),
            typeof(WindowDragBehavior),
            new PropertyMetadata(false, OnEnableDragChanged));

    public static bool GetEnableDrag(DependencyObject obj)
    {
        return (bool)obj.GetValue(EnableDragProperty);
    }

    public static void SetEnableDrag(DependencyObject obj, bool value)
    {
        obj.SetValue(EnableDragProperty, value);
    }

    private static void OnEnableDragChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is UIElement element)
        {
            if ((bool)e.NewValue)
                element.MouseLeftButtonDown += Element_MouseLeftButtonDown;
            else
                element.MouseLeftButtonDown -= Element_MouseLeftButtonDown;
        }
    }

    private static void Element_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is DependencyObject dependencyObject)
        {
            Window window = Window.GetWindow(dependencyObject);
            window?.DragMove();
        }
    }
}
