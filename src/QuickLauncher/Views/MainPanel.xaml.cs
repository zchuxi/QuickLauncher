using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using QuickLauncher.ViewModels;

namespace QuickLauncher.Views;

/// <summary>
/// 环形主面板控件。
/// </summary>
public sealed partial class MainPanel : UserControl
{
    public MainViewModel ViewModel { get; }

    public MainPanel()
    {
        this.InitializeComponent();
        ViewModel = new MainViewModel();
    }

    public MainPanel(MainViewModel viewModel)
    {
        this.InitializeComponent();
        ViewModel = viewModel;
    }
}
