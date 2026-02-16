using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DustInTheWind.WpfToolkit;

public class SideBarItem : ContentControl
{
    #region Header DependencyProperty

    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
        nameof(Header),
        typeof(object),
        typeof(SideBarItem),
        new PropertyMetadata(null)
    );

    public object Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    #endregion

    #region HeaderTemplate DependencyProperty

    public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(
        nameof(HeaderTemplate),
        typeof(DataTemplate),
        typeof(SideBarItem),
        new PropertyMetadata(null)
    );

    public DataTemplate HeaderTemplate
    {
        get => (DataTemplate)GetValue(HeaderTemplateProperty);
        set => SetValue(HeaderTemplateProperty, value);
    }

    #endregion

    #region IsSelected DependencyProperty

    public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
        nameof(IsSelected),
        typeof(bool),
        typeof(SideBarItem),
        new PropertyMetadata(false, OnIsSelectedChanged)
    );

    private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is SideBarItem sideBarItem)
            sideBarItem.OnIsSelectedChanged((bool)e.NewValue);
    }

    public bool IsSelected
    {
        get => (bool)GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    #endregion

    #region SelectedBackground DependencyProperty

    public static readonly DependencyProperty SelectedBackgroundProperty = DependencyProperty.Register(
        nameof(SelectedBackground),
        typeof(Brush),
        typeof(SideBarItem),
        new PropertyMetadata(null)
    );

    public Brush SelectedBackground
    {
        get => (Brush)GetValue(SelectedBackgroundProperty);
        set => SetValue(SelectedBackgroundProperty, value);
    }

    #endregion

    #region MouseOverBackground DependencyProperty

    public static readonly DependencyProperty MouseOverBackgroundProperty = DependencyProperty.Register(
        nameof(MouseOverBackground),
        typeof(Brush),
        typeof(SideBarItem),
        new PropertyMetadata(null)
    );

    public Brush MouseOverBackground
    {
        get => (Brush)GetValue(MouseOverBackgroundProperty);
        set => SetValue(MouseOverBackgroundProperty, value);
    }

    #endregion

    static SideBarItem()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(SideBarItem), new FrameworkPropertyMetadata(typeof(SideBarItem)));
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);

        if (e.Key == Key.Space || e.Key == Key.Enter)
        {
            SelectThisItem();
            e.Handled = true;
        }
    }

    protected override void OnMouseLeftButtonDown(System.Windows.Input.MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonDown(e);

        SelectThisItem();
        e.Handled = true;
    }

    private void SelectThisItem()
    {
        SideBar sideBar = FindParentSideBar();
        
        if (sideBar != null)
        {
            object item = sideBar.ItemContainerGenerator.ItemFromContainer(this);

            sideBar.SelectedItem = item == DependencyProperty.UnsetValue
                ? this
                : item;
        }
    }

    private SideBar FindParentSideBar()
    {
        DependencyObject current = this;

        while (current != null)
        {
            current = VisualTreeHelper.GetParent(current);

            if (current is SideBar sideBar)
                return sideBar;
        }

        return null;
    }

    protected virtual void OnIsSelectedChanged(bool isSelected)
    {
        // Can be overridden for custom behavior when selection changes
    }
}
