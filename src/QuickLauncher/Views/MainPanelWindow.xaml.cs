using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;

namespace QuickLauncher.Views;

/// <summary>
/// 承载环形主面板的独立窗口。
/// </summary>
public sealed partial class MainPanelWindow : Window
{
    public MainPanelWindow()
    {
        this.InitializeComponent();
        this.Title = "环形启动器";
        this.SystemBackdrop = new MicaBackdrop();
    }
}
