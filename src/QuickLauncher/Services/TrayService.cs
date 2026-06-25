using H.NotifyIcon;

namespace QuickLauncher.Services;

/// <summary>
/// 基于 H.NotifyIcon.WinUI 的系统托盘服务。
/// </summary>
public sealed class TrayService : ITrayService
{
    private TaskbarIcon? _icon;
    private bool _initialized;

    public event EventHandler? ShowPanelClicked;
    public event EventHandler? SettingsClicked;
    public event EventHandler? ExitClicked;

    public void Initialize()
    {
        if (_initialized) return;
        _icon = new TaskbarIcon
        {
            ToolTipText = "鼠标极速启动器",
            // 无图标资源时使用默认；正式版应设置 IconSource
        };
        _initialized = true;
    }

    public void Show()
    {
        Initialize();
        // H.NotifyIcon 在创建 TaskbarIcon 后即显示，无需 ForceUpdate
    }

    public void Hide()
    {
        _icon?.Dispose();
        _icon = null;
        _initialized = false;
    }

    public void ShowTooltip(string title, string message)
    {
        _icon?.ShowNotification(title, message);
    }

    /// <summary>供托盘菜单项调用。</summary>
    internal void RaiseShowPanel() => ShowPanelClicked?.Invoke(this, EventArgs.Empty);

    internal void RaiseSettings() => SettingsClicked?.Invoke(this, EventArgs.Empty);

    internal void RaiseExit() => ExitClicked?.Invoke(this, EventArgs.Empty);
}
