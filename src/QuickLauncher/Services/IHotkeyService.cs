using QuickLauncher.Models;

namespace QuickLauncher.Services;

/// <summary>
/// 全局快捷键服务接口。
/// </summary>
public interface IHotkeyService
{
    /// <summary>注册一个快捷键。返回是否成功。</summary>
    bool Register(HotkeyConfig config);

    /// <summary>取消注册指定 Id 的快捷键。</summary>
    bool Unregister(string id);

    /// <summary>取消全部注册。</summary>
    void UnregisterAll();

    /// <summary>当前已注册的快捷键。</summary>
    IReadOnlyList<HotkeyConfig> Registered { get; }

    /// <summary>快捷键被触发。</summary>
    event EventHandler<HotkeyTriggeredEventArgs>? HotkeyTriggered;
}

public sealed class HotkeyTriggeredEventArgs : EventArgs
{
    public HotkeyConfig Config { get; init; } = null!;
}
