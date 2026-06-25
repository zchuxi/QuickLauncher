using Microsoft.UI.Xaml;
using QuickLauncher.Services;

namespace QuickLauncher;

/// <summary>
/// 应用程序入口。
/// </summary>
public partial class App : Application
{
    private Views.MainWindow? _mainWindow;

    public App()
    {
        this.InitializeComponent();
    }

    /// <summary>
    /// 应用启动时调用。
    /// </summary>
    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        // 初始化服务
        ServiceLocator.Tray.Initialize();
        ServiceLocator.Tray.ShowPanelClicked += OnTrayShowPanel;
        ServiceLocator.Tray.SettingsClicked += OnTraySettings;
        ServiceLocator.Tray.ExitClicked += OnTrayExit;

        // 注册默认快捷键
        try
        {
            ServiceLocator.Hotkey.EnsureInitialized();
            foreach (var hk in Data.MockData.Hotkeys)
            {
                ServiceLocator.Hotkey.Register(hk);
            }
            ServiceLocator.Hotkey.HotkeyTriggered += OnHotkeyTriggered;
        }
        catch
        {
            // unpackaged 模式下注册可能失败，忽略
        }

        // 启动鼠标钩子并注册边缘手势
        try
        {
            foreach (var g in ServiceLocator.Settings.Current.EdgeGestures)
            {
                ServiceLocator.MouseHook.RegisterEdge(g);
            }
            ServiceLocator.MouseHook.EdgeEntered += OnEdgeEntered;
            ServiceLocator.MouseHook.Start();
        }
        catch
        {
            // 钩子启动失败不阻塞
        }

        _mainWindow = new Views.MainWindow();
        _mainWindow.Activate();
        ServiceLocator.Tray.Show();
    }

    private Views.MainPanelWindow? _panelWindow;

    private void OnTrayShowPanel(object? sender, System.EventArgs e) => ShowPanel();

    private void OnHotkeyTriggered(object? sender, HotkeyTriggeredEventArgs e)
    {
        if (e.Config.Action == Models.HotkeyAction.ShowPanel)
        {
            ShowPanel();
        }
        else if (e.Config.Action == Models.HotkeyAction.HidePanel)
        {
            _panelWindow?.Close();
        }
    }

    private void OnEdgeEntered(object? sender, EdgeEnteredEventArgs e)
    {
        if (e.Action == Models.GestureAction.OpenPanel) ShowPanel();
    }

    private void ShowPanel()
    {
        _mainWindow?.DispatcherQueue.TryEnqueue(() =>
        {
            if (_panelWindow is null)
            {
                _panelWindow = new Views.MainPanelWindow();
                _panelWindow.Closed += (_, _) => _panelWindow = null;
            }
            _panelWindow.Activate();
        });
    }

    private void OnTraySettings(object? sender, System.EventArgs e)
    {
        _mainWindow?.DispatcherQueue.TryEnqueue(async () =>
        {
            if (_mainWindow is null) return;
            var dialog = new Views.SettingsDialog { XamlRoot = _mainWindow.Content.XamlRoot };
            await dialog.ShowAsync();
        });
    }

    private void OnTrayExit(object? sender, System.EventArgs e)
    {
        ServiceLocator.MouseHook.Stop();
        ServiceLocator.Hotkey.UnregisterAll();
        ServiceLocator.Tray.Hide();
        _mainWindow?.Close();
    }
}
