namespace QuickLauncher.Services;

/// <summary>
/// 系统托盘服务接口。
/// </summary>
public interface ITrayService
{
    void Initialize();
    void Show();
    void Hide();
    void ShowTooltip(string title, string message);

    event EventHandler? ShowPanelClicked;
    event EventHandler? SettingsClicked;
    event EventHandler? ExitClicked;
}
