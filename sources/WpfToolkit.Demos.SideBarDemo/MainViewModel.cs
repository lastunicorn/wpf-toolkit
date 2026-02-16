using DustInTheWind.WpfToolkit.Demos.SideBarDemo.Commands;
using DustInTheWind.WpfToolkit.Demos.SideBarDemo.Pages;

namespace DustInTheWind.WpfToolkit.Demos.SideBarDemo;

public class MainViewModel : ViewModelBase
{
    public TrianglePageModel TrianglePageModel { get; }

    public SquarePageModel SquarePageModel { get; }

    public CirclePageModel CirclePageModel { get; }

    public OneTwoThreeCommand OneTwoThreeCommand { get; }

    public FourFiveSixCommand FourFiveSixCommand { get; }

    public MainViewModel()
    {
        TrianglePageModel = new();
        SquarePageModel = new();
        CirclePageModel = new();

        OneTwoThreeCommand = new();
        FourFiveSixCommand = new();
    }
}
