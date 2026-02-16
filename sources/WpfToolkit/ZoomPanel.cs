using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DustInTheWind.WpfToolkit;

public class ZoomPanel : ContentControl
{
    #region ZoomValue DependencyProperty

    public static readonly DependencyProperty ZoomValueProperty = DependencyProperty.Register(
        nameof(ZoomValue),
        typeof(double),
        typeof(ZoomPanel),
        new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, HandleZoomValueChanged));

    private static void HandleZoomValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ZoomPanel zoomPanControl)
        {
            double oldZoomValue = (double)e.OldValue;
            double newZoomValue = (double)e.NewValue;

            if (zoomPanControl.scaleTransform != null)
            {
                zoomPanControl.scaleTransform.ScaleX = newZoomValue;
                zoomPanControl.scaleTransform.ScaleY = newZoomValue;
            }

            // If zoom was not triggered by mouse wheel, adjust location to keep center point fixed
            if (!zoomPanControl.isZoomingWithMouseWheel &&
                zoomPanControl.containerElement != null &&
                zoomPanControl.contentElement != null &&
                zoomPanControl.translateTransform != null &&
                oldZoomValue != 0)
            {
                Point oldLocation = zoomPanControl.Location;

                double centerX = zoomPanControl.containerElement.ActualWidth / 2;
                double centerY = zoomPanControl.containerElement.ActualHeight / 2;

                double offsetX = (zoomPanControl.containerElement.ActualWidth - zoomPanControl.contentElement.ActualWidth) / 2;
                double offsetY = (zoomPanControl.containerElement.ActualHeight - zoomPanControl.contentElement.ActualHeight) / 2;

                double centerRelativeToTargetX = centerX - offsetX;
                double centerRelativeToTargetY = centerY - offsetY;

                double zoomFactor = newZoomValue / oldZoomValue;

                double x = centerRelativeToTargetX - (centerRelativeToTargetX - oldLocation.X) * zoomFactor;
                double y = centerRelativeToTargetY - (centerRelativeToTargetY - oldLocation.Y) * zoomFactor;

                zoomPanControl.Location = new Point(x, y);
            }
        }
    }

    public double ZoomValue
    {
        get => (double)GetValue(ZoomValueProperty);
        set => SetValue(ZoomValueProperty, value);
    }

    #endregion

    #region Location DependencyProperty

    public static readonly DependencyProperty LocationProperty = DependencyProperty.Register(
        nameof(Location),
        typeof(Point),
        typeof(ZoomPanel),
        new FrameworkPropertyMetadata(new Point(0, 0), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, HandleLocationChanged));

    private static void HandleLocationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ZoomPanel zoomPanControl)
        {
            Point newLocation = (Point)e.NewValue;

            if (zoomPanControl.translateTransform != null)
            {
                zoomPanControl.translateTransform.X = newLocation.X;
                zoomPanControl.translateTransform.Y = newLocation.Y;
            }
        }
    }

    public Point Location
    {
        get => (Point)GetValue(LocationProperty);
        set => SetValue(LocationProperty, value);
    }

    #endregion

    #region ShowZoomSlider DependencyProperty

    public static readonly DependencyProperty ShowZoomSliderProperty = DependencyProperty.Register(
        nameof(ShowZoomSlider),
        typeof(bool),
        typeof(ZoomPanel),
        new FrameworkPropertyMetadata(true));

    public bool ShowZoomSlider
    {
        get => (bool)GetValue(ShowZoomSliderProperty);
        set => SetValue(ShowZoomSliderProperty, value);
    }

    #endregion

    #region ShowResetButton DependencyProperty

    public static readonly DependencyProperty ShowResetButtonProperty = DependencyProperty.Register(
        nameof(ShowResetButton),
        typeof(bool),
        typeof(ZoomPanel),
        new FrameworkPropertyMetadata(true));

    public bool ShowResetButton
    {
        get => (bool)GetValue(ShowResetButtonProperty);
        set => SetValue(ShowResetButtonProperty, value);
    }

    #endregion

    #region ResetButtonTemplate DependencyProperty

    public static readonly DependencyProperty ResetButtonTemplateProperty = DependencyProperty.Register(
        nameof(ResetButtonTemplate),
        typeof(ControlTemplate),
        typeof(ZoomPanel),
        new FrameworkPropertyMetadata(null));

    public ControlTemplate ResetButtonTemplate
    {
        get => (ControlTemplate)GetValue(ResetButtonTemplateProperty);
        set => SetValue(ResetButtonTemplateProperty, value);
    }

    #endregion

    #region ResetViewCommand DependencyProperty

    public static readonly DependencyProperty ResetViewCommandProperty = DependencyProperty.Register(
        nameof(ResetViewCommand),
        typeof(ICommand),
        typeof(ZoomPanel));

    internal ICommand ResetViewCommand
    {
        get => (ICommand)GetValue(ResetViewCommandProperty);
        private set => SetValue(ResetViewCommandProperty, value);
    }

    #endregion

    #region CornerRadius DependencyProperty

    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        nameof(CornerRadius),
        typeof(CornerRadius),
        typeof(ZoomPanel),
        new FrameworkPropertyMetadata(new CornerRadius(0)));

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    #endregion

    #region ShowCoordinates DependencyProperty

    public static readonly DependencyProperty ShowCoordinatesProperty = DependencyProperty.Register(
        nameof(ShowCoordinates),
        typeof(bool),
        typeof(ZoomPanel),
        new FrameworkPropertyMetadata(true));

    public bool ShowCoordinates
    {
        get => (bool)GetValue(ShowCoordinatesProperty);
        set => SetValue(ShowCoordinatesProperty, value);
    }

    #endregion

    #region MinZoom DependencyProperty

    public static readonly DependencyProperty MinZoomProperty = DependencyProperty.Register(
        nameof(MinZoom),
        typeof(double),
        typeof(ZoomPanel),
        new FrameworkPropertyMetadata(0.1));

    public double MinZoom
    {
        get => (double)GetValue(MinZoomProperty);
        set => SetValue(MinZoomProperty, value);
    }

    #endregion

    #region MaxZoom DependencyProperty

    public static readonly DependencyProperty MaxZoomProperty = DependencyProperty.Register(
        nameof(MaxZoom),
        typeof(double),
        typeof(ZoomPanel),
        new FrameworkPropertyMetadata(5.0));

    public double MaxZoom
    {
        get => (double)GetValue(MaxZoomProperty);
        set => SetValue(MaxZoomProperty, value);
    }

    #endregion

    #region ZoomIncrement DependencyProperty

    public static readonly DependencyProperty ZoomIncrementProperty = DependencyProperty.Register(
        nameof(ZoomIncrement),
        typeof(double),
        typeof(ZoomPanel),
        new FrameworkPropertyMetadata(0.1));

    public double ZoomIncrement
    {
        get => (double)GetValue(ZoomIncrementProperty);
        set => SetValue(ZoomIncrementProperty, value);
    }

    #endregion

    #region SliderLength DependencyProperty

    public static readonly DependencyProperty SliderLengthProperty = DependencyProperty.Register(
        nameof(SliderLength),
        typeof(double),
        typeof(ZoomPanel),
        new FrameworkPropertyMetadata(100.0));

    public double SliderLength
    {
        get => (double)GetValue(SliderLengthProperty);
        set => SetValue(SliderLengthProperty, value);
    }

    #endregion

    static ZoomPanel()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ZoomPanel), new FrameworkPropertyMetadata(typeof(ZoomPanel)));
    }

    private bool isDragging;
    internal bool isZoomingWithMouseWheel;
    private Point lastMousePosition;
    private FrameworkElement containerElement;
    private FrameworkElement contentElement;
    private ScaleTransform scaleTransform;
    private TranslateTransform translateTransform;

    public ZoomPanel()
    {
        ResetViewCommand = new ResetViewCommand(this);
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        isDragging = false;
        isZoomingWithMouseWheel = false;
        lastMousePosition = new Point(0, 0);
        containerElement = null;
        contentElement = null;
        scaleTransform = null;
        translateTransform = null;
        ZoomValue = 1.0;

        if (GetTemplateChild("PART_Content") is FrameworkElement newContentElement)
        {
            scaleTransform = new ScaleTransform(ZoomValue, ZoomValue);
            translateTransform = new TranslateTransform(0.0, 0.0);

            TransformGroup transformGroup = new();

            transformGroup.Children.Add(scaleTransform);
            transformGroup.Children.Add(translateTransform);

            newContentElement.RenderTransform = transformGroup;

            contentElement = newContentElement;
        }

        if (containerElement != null)
        {
            containerElement.MouseWheel -= HandleMouseWheel;
            containerElement.MouseLeftButtonDown -= HandleMouseLeftButtonDown;
            containerElement.MouseLeftButtonUp -= HandleMouseLeftButtonUp;
            containerElement.MouseMove -= HandleMouseMove;
        }

        if (GetTemplateChild("PART_Container") is FrameworkElement newContainerElement)
        {
            newContainerElement.MouseWheel += HandleMouseWheel;
            newContainerElement.MouseLeftButtonDown += HandleMouseLeftButtonDown;
            newContainerElement.MouseLeftButtonUp += HandleMouseLeftButtonUp;
            newContainerElement.MouseMove += HandleMouseMove;

            containerElement = newContainerElement;
        }
    }

    private void HandleMouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (sender is not FrameworkElement element)
            return;

        if (scaleTransform == null || translateTransform == null)
            return;

        isZoomingWithMouseWheel = true;

        Point oldLocation = Location;

        double oldZoom = ZoomValue;
        double zoomDelta = e.Delta > 0 ? ZoomIncrement : -ZoomIncrement;
        double newZoom = oldZoom + zoomDelta;

        newZoom = Math.Max(MinZoom, Math.Min(MaxZoom, newZoom));

        Point mousePosition = e.GetPosition(element);

        double offsetX = (element.ActualWidth - contentElement.ActualWidth) / 2;
        double offsetY = (element.ActualHeight - contentElement.ActualHeight) / 2;

        double mouseRelativeToTargetX = mousePosition.X - offsetX;
        double mouseRelativeToTargetY = mousePosition.Y - offsetY;

        double zoomFactor = newZoom / oldZoom;

        double x = mouseRelativeToTargetX - (mouseRelativeToTargetX - oldLocation.X) * zoomFactor;
        double y = mouseRelativeToTargetY - (mouseRelativeToTargetY - oldLocation.Y) * zoomFactor;

        Location = new Point(x, y);
        ZoomValue = newZoom;

        isZoomingWithMouseWheel = false;

        e.Handled = true;
    }

    private void HandleMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is not FrameworkElement element)
            return;

        isDragging = true;
        lastMousePosition = e.GetPosition(element);

        element.CaptureMouse();

        e.Handled = true;
    }

    private void HandleMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (sender is not FrameworkElement element)
            return;

        if (isDragging)
        {
            isDragging = false;
            element.ReleaseMouseCapture();
            e.Handled = true;
        }
    }

    private void HandleMouseMove(object sender, MouseEventArgs e)
    {
        if (sender is not FrameworkElement element)
            return;

        if (!isDragging)
            return;

        if (translateTransform == null)
            return;

        Point oldLocation = Location;

        Point currentPosition = e.GetPosition(element);
        double deltaX = currentPosition.X - lastMousePosition.X;
        double deltaY = currentPosition.Y - lastMousePosition.Y;

        double x = oldLocation.X + deltaX;
        double y = oldLocation.Y + deltaY;

        Location = new Point(x, y);

        lastMousePosition = currentPosition;

        e.Handled = true;
    }

    internal void Reset()
    {
        ZoomValue = 1.0;
        Location = new Point(0, 0);
    }
}
