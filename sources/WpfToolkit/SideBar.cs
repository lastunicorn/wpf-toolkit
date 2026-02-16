using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace DustInTheWind.WpfToolkit;

public class SideBar : Selector
{
    #region Buttons DependencyProperty

    public static readonly DependencyProperty ButtonsProperty = DependencyProperty.Register(
        nameof(Buttons),
        typeof(ObservableCollection<Button>),
        typeof(SideBar),
        new PropertyMetadata(new ObservableCollection<Button>())
    );

    public ObservableCollection<Button> Buttons
    {
        get => (ObservableCollection<Button>)GetValue(ButtonsProperty);
        set => SetValue(ButtonsProperty, value);
    }

    #endregion

    #region ItemSelectedBackground DependencyProperty

    public static readonly DependencyProperty ItemSelectedBackgroundProperty = DependencyProperty.Register(
        nameof(ItemSelectedBackground),
        typeof(Brush),
        typeof(SideBar),
        new PropertyMetadata(null)
    );

    public Brush ItemSelectedBackground
    {
        get => (Brush)GetValue(ItemSelectedBackgroundProperty);
        set => SetValue(ItemSelectedBackgroundProperty, value);
    }

    #endregion

    #region ItemMouseOverBackground DependencyProperty

    public static readonly DependencyProperty ItemMouseOverBackgroundProperty = DependencyProperty.Register(
        nameof(ItemMouseOverBackground),
        typeof(Brush),
        typeof(SideBar),
        new PropertyMetadata(null)
    );

    public Brush ItemMouseOverBackground
    {
        get => (Brush)GetValue(ItemMouseOverBackgroundProperty);
        set => SetValue(ItemMouseOverBackgroundProperty, value);
    }

    #endregion

    #region SelectedContent DependencyProperty

    private static readonly DependencyPropertyKey SelectedContentPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(SelectedContent),
        typeof(object),
        typeof(SideBar),
        new PropertyMetadata(null)
    );

    public static readonly DependencyProperty SelectedContentProperty = SelectedContentPropertyKey.DependencyProperty;

    public object SelectedContent
    {
        get => GetValue(SelectedContentProperty);
        private set => SetValue(SelectedContentPropertyKey, value);
    }

    #endregion

    static SideBar()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(SideBar), new FrameworkPropertyMetadata(typeof(SideBar)));
    }

    public SideBar()
    {
        Buttons = [];
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (SelectedItem == null)
        {
            foreach (object item in Items)
            {
                if (item is SideBarItem sideBarItem && sideBarItem.IsSelected)
                {
                    SelectedItem = item;
                    break;
                }
            }
        }
        else
        {
            UpdateSelectedContent();
        }
    }

    protected override void OnSelectionChanged(SelectionChangedEventArgs e)
    {
        base.OnSelectionChanged(e);

        UpdateSelectedContent();
        
        foreach (object item in e.RemovedItems)
        {
            SideBarItem container = ItemContainerGenerator.ContainerFromItem(item) as SideBarItem;
            
            if (container != null)
                container.IsSelected = false;
        }
        
        foreach (object item in e.AddedItems)
        {
            SideBarItem container = ItemContainerGenerator.ContainerFromItem(item) as SideBarItem;
            
            if (container != null)
                container.IsSelected = true;
        }
    }

    protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
    {
        base.OnItemsChanged(e);

        if (e.Action == NotifyCollectionChangedAction.Add && SelectedItem == null)
        {
            foreach (object item in e.NewItems)
            {
                if (item is SideBarItem sideBarItem && sideBarItem.IsSelected)
                {
                    SelectedItem = item;
                    break;
                }
            }
        }
    }

    private void UpdateSelectedContent()
    {
        if (SelectedItem is SideBarItem selectedItem)
            SelectedContent = selectedItem.Content;
        else
            SelectedContent = null;
    }

    protected override DependencyObject GetContainerForItemOverride()
    {
        return new SideBarItem();
    }

    protected override bool IsItemItsOwnContainerOverride(object item)
    {
        return item is SideBarItem;
    }
}
