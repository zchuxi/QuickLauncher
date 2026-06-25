namespace QuickLauncher.Models;

/// <summary>
/// 屏幕边缘。
/// </summary>
public enum ScreenEdge
{
    Top,
    Bottom,
    Left,
    Right,
}

/// <summary>
/// 鼠标手势动作。
/// </summary>
public enum GestureAction
{
    None,
    SwitchToFavorites,
    SwitchToRecent,
    SwitchToDevelopment,
    OpenPanel,
    ClosePanel,
}

/// <summary>
/// 主题模式。
/// </summary>
public enum ThemeMode
{
    Light,
    Dark,
    System,
}

/// <summary>
/// 单个边缘手势配置。
/// </summary>
public sealed class EdgeGestureSetting
{
    public ScreenEdge Edge { get; set; }

    public bool Enabled { get; set; }

    public double Threshold { get; set; } = 10;

    public GestureAction Action { get; set; }

    public string EdgeDisplayName => Edge switch
    {
        ScreenEdge.Top => "上边缘",
        ScreenEdge.Bottom => "下边缘",
        ScreenEdge.Left => "左边缘",
        ScreenEdge.Right => "右边缘",
        _ => Edge.ToString(),
    };

    public string ActionDisplayName => Action switch
    {
        GestureAction.None => "无",
        GestureAction.SwitchToFavorites => "切换到收藏",
        GestureAction.SwitchToRecent => "切换到最近",
        GestureAction.SwitchToDevelopment => "切换到开发",
        GestureAction.OpenPanel => "打开面板",
        GestureAction.ClosePanel => "关闭面板",
        _ => Action.ToString(),
    };
}

/// <summary>
/// 应用设置模型。
/// </summary>
public sealed class SettingsModel
{
    public ThemeMode Theme { get; set; } = ThemeMode.System;

    /// <summary>面板透明度 0.5 ~ 1.0。</summary>
    public double PanelOpacity { get; set; } = 0.85;

    public bool AnimationsEnabled { get; set; } = true;

    public string TriggerHotkey { get; set; } = "Alt+Space";

    public List<EdgeGestureSetting> EdgeGestures { get; set; } = new();

    public bool StartWithWindows { get; set; }

    public bool MinimizeToTray { get; set; } = true;

    public bool ShowInTaskbar { get; set; }

    public string AppVersion { get; set; } = "1.0.0.0 (原型)";
}
