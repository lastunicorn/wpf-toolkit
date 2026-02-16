using System.Windows;

namespace DustInTheWind.WpfToolkit;

internal class ZoomPanState
{
    public bool IsDragging { get; set; }

    public Point LastMousePosition { get; set; }
}
