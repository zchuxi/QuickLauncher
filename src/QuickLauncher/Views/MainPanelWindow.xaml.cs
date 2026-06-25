using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Windowing;

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
        this.SystemBackdrop = new MicaBackdrop { Kind = MicaKind.BaseAlt };

        // WinUI 3 的 Window 不支持在 XAML 中设置 Width/Height，需通过 AppWindow.Resize
        this.AppWindow.Resize(new SizeInt32(720, 720));
    }
}
