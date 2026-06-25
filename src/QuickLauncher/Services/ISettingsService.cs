using QuickLauncher.Models;

namespace QuickLauncher.Services;

/// <summary>
/// 设置服务接口：持久化应用设置。
/// </summary>
public interface ISettingsService
{
    SettingsModel Current { get; }
    void Save();
    void Reload();
    event EventHandler? SettingsChanged;
}
