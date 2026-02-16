using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace DustInTheWind.WpfToolkit;

public static class ZoomPanBehavior
{
    private const double ZoomIncrement = 0.1;
    private const double MinZoom = 0.1;
    private const double MaxZoom = 5.0;

    #region State Dependency Property

    private static readonly DependencyProperty StateProperty = DependencyProperty.RegisterAttached(
        "State",
        typeof(ZoomPanState),
        typeof(ZoomPanBehavior),
        new PropertyMetadata(null));

    #endregion

    #region IsEnabled Dependency Property

    public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
        "IsEnabled",
        typeof(bool),
        typeof(ZoomPanBehavior),
        new PropertyMetadata(false, OnIsEnabledChanged));

    private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not FrameworkElement element)
            return;

        if ((bool)e.NewValue)
        {
            ZoomPanState state = new();
            element.SetValue(StateProperty, state);

            element.MouseWheel += HandleMouseWheel;
            element.MouseLeftButtonDown += HandleMouseLeftButtonDown;
            element.MouseLeftButtonUp += HandleMouseLeftButtonUp;
            element.MouseMove += HandleMouseMove;
        }
        else
        {
            element.MouseWheel -= HandleMouseWheel;
            element.MouseLeftButtonDown -= HandleMouseLeftButtonDown;
            element.MouseLeftButtonUp -= HandleMouseLeftButtonUp;
            element.MouseMove -= HandleMouseMove;

            element.ClearValue(StateProperty);
        }
    }

    public static bool GetIsEnabled(DependencyObject obj)
    {
        return (bool)obj.GetValue(IsEnabledProperty);
    }

    public static void SetIsEnabled(DependencyObject obj, bool value)
    {
        obj.SetValue(IsEnabledProperty, value);
    }

    #endregion

    #region Target Dependency Property

    public static readonly DependencyProperty TargetProperty = DependencyProperty.RegisterAttached(
        "Target",
        typeof(FrameworkElement),
        typeof(ZoomPanBehavior),
        new PropertyMetadata(null));

    public static FrameworkElement GetTarget(DependencyObject obj)
    {
        return (FrameworkElement)obj.GetValue(TargetProperty);
    }

    public static void SetTarget(DependencyObject obj, FrameworkElement value)
    {
        obj.SetValue(TargetProperty, value);
    }

    #endregion

    private static void HandleMouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (sender is not FrameworkElement element)
            return;

        FrameworkElement target = GetTarget(element) ?? element;

        if (target.RenderTransform is not TransformGroup transformGroup)
            return;

        ScaleTransform scaleTransform = FindScaleTransform(transformGroup);
        TranslateTransform translateTransform = FindTranslateTransform(transformGroup);

        if (scaleTransform == null || translateTransform == null)
            return;

        double oldZoom = scaleTransform.ScaleX;
        double zoomDelta = e.Delta > 0 ? ZoomIncrement : -ZoomIncrement;
        double newZoom = oldZoom + zoomDelta;

        newZoom = Math.Max(MinZoom, Math.Min(MaxZoom, newZoom));

        Point mousePosition = e.GetPosition(element);
        
        double offsetX = (element.ActualWidth - target.ActualWidth) / 2;
        double offsetY = (element.ActualHeight - target.ActualHeight) / 2;
        
        double mouseRelativeToTargetX = mousePosition.X - offsetX;
        double mouseRelativeToTargetY = mousePosition.Y - offsetY;

        double zoomFactor = newZoom / oldZoom;
        
        translateTransform.X = mouseRelativeToTargetX - (mouseRelativeToTargetX - translateTransform.X) * zoomFactor;
        translateTransform.Y = mouseRelativeToTargetY - (mouseRelativeToTargetY - translateTransform.Y) * zoomFactor;

        scaleTransform.ScaleX = newZoom;
        scaleTransform.ScaleY = newZoom;

        e.Handled = true;
    }

    private static void HandleMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is not FrameworkElement element)
            return;

        ZoomPanState state = (ZoomPanState)element.GetValue(StateProperty);

        state.IsDragging = true;
        state.LastMousePosition = e.GetPosition(element);

        element.CaptureMouse();

        e.Handled = true;
    }

    private static void HandleMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (sender is not FrameworkElement element)
            return;

        ZoomPanState state = (ZoomPanState)element.GetValue(StateProperty);

        if (state.IsDragging)
        {
            state.IsDragging = false;
            element.ReleaseMouseCapture();
            e.Handled = true;
        }
    }

    private static void HandleMouseMove(object sender, MouseEventArgs e)
    {
        if (sender is not FrameworkElement element)
            return;

        ZoomPanState state = (ZoomPanState)element.GetValue(StateProperty);

        if (!state.IsDragging)
            return;

        FrameworkElement target = GetTarget(element) ?? element;

        if (target.RenderTransform is not TransformGroup transformGroup)
            return;

        TranslateTransform translateTransform = FindTranslateTransform(transformGroup);

        if (translateTransform == null)
            return;

        Point currentPosition = e.GetPosition(element);
        double deltaX = currentPosition.X - state.LastMousePosition.X;
        double deltaY = currentPosition.Y - state.LastMousePosition.Y;

        translateTransform.X += deltaX;
        translateTransform.Y += deltaY;

        state.LastMousePosition = currentPosition;

        e.Handled = true;
    }

    private static ScaleTransform FindScaleTransform(TransformGroup transformGroup)
    {
        foreach (Transform transform in transformGroup.Children)
        {
            if (transform is ScaleTransform scaleTransform)
                return scaleTransform;
        }

        return null;
    }

    private static TranslateTransform FindTranslateTransform(TransformGroup transformGroup)
    {
        foreach (Transform transform in transformGroup.Children)
        {
            if (transform is TranslateTransform translateTransform)
                return translateTransform;
        }

        return null;
    }
}
