using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace QuickLauncher.Views;

/// <summary>
/// 主窗口。原型阶段作为入口，承载导航按钮打开各页面。
/// </summary>
public sealed partial class MainWindow : Window
{
    private MainPanelWindow? _panelWindow;

    public MainWindow()
    {
        this.InitializeComponent();
        this.Title = "鼠标极速启动器 - 原型";
        this.SystemBackdrop = new MicaBackdrop();
    }

    /// <summary>显示主面板（独立窗口）。</summary>
    private void ShowMainPanel_Click(object sender, RoutedEventArgs e)
    {
        if (_panelWindow is null)
        {
            _panelWindow = new MainPanelWindow();
            _panelWindow.Closed += (_, _) => _panelWindow = null;
        }
        _panelWindow.Activate();
    }

    /// <summary>显示应用浏览器。</summary>
    private async void ShowAppBrowser_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new AppBrowserDialog { XamlRoot = this.Content.XamlRoot };
        await dialog.ShowAsync();
    }

    /// <summary>显示快捷键设置。</summary>
    private async void ShowHotkeySettings_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new HotkeySettingsDialog { XamlRoot = this.Content.XamlRoot };
        await dialog.ShowAsync();
    }

    /// <summary>显示设置面板。</summary>
    private async void ShowSettings_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new SettingsDialog { XamlRoot = this.Content.XamlRoot };
        await dialog.ShowAsync();
    }
}
