using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DustInTheWind.WpfToolkit;

public class ViewModelBase : INotifyPropertyChanged
{
    protected bool IsInitializing { get; private set; }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected Task Initialize(Func<Task> action)
    {
        IsInitializing = true;

        try
        {
            return action?.Invoke();
        }
        finally
        {
            IsInitializing = false;
        }
    }

    protected void Initialize(Action action)
    {
        IsInitializing = true;

        try
        {
            action?.Invoke();
        }
        finally
        {
            IsInitializing = false;
        }
    }
}