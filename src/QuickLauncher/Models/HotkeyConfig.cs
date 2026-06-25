namespace QuickLauncher.Models;

/// <summary>
/// 快捷键动作类型。
/// </summary>
public enum HotkeyAction
{
    /// <summary>显示主面板</summary>
    ShowPanel,

    /// <summary>隐藏主面板</summary>
    HidePanel,

    /// <summary>启动指定应用</summary>
    LaunchApp,

    /// <summary>切换分类</summary>
    SwitchCategory,
}

/// <summary>
/// 快捷键配置模型。
/// </summary>
public sealed class HotkeyConfig
{
    public string Id { get; init; } = Guid.NewGuid().ToString();

    /// <summary>显示用按键串，如 "Alt+Space"。</summary>
    public string DisplayKey { get; init; } = string.Empty;

    public HotkeyAction Action { get; init; }

    /// <summary>关联应用名称（LaunchApp 时有效）。</summary>
    public string? AppName { get; init; }

    /// <summary>关联应用图标 glyph。</summary>
    public string AppIconGlyph { get; init; } = "\uE7B8";

    /// <summary>动作显示文本。</summary>
    public string ActionDescription => Action switch
    {
        HotkeyAction.ShowPanel => "显示面板",
        HotkeyAction.HidePanel => "隐藏面板",
        HotkeyAction.LaunchApp => AppName is null ? "启动应用" : $"启动 {AppName}",
        HotkeyAction.SwitchCategory => "切换分类",
        _ => Action.ToString(),
    };
}
