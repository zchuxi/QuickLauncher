using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using QuickLauncher.Models;
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

    /// <summary>外环应用图标点击：启动应用。</summary>
    private void RingApp_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.DataContext is AppInfo app)
        {
            ViewModel.LaunchAppCommand.Execute(app);
        }
    }
}
