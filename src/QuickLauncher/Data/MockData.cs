using QuickLauncher.Models;

namespace QuickLauncher.Data;

/// <summary>
/// 原型阶段使用的静态 Mock 数据。
/// 图标使用 Segoe Fluent Icons 字符代码。
/// </summary>
public static class MockData
{
    /// <summary>所有应用列表。</summary>
    public static IReadOnlyList<AppInfo> Apps { get; } = new List<AppInfo>
    {
        // Development
        new() { Id = "vscode", Name = "Visual Studio Code", IconGlyph = "\uE943", Category = AppCategory.Development, LaunchCount = 320, LastUsed = DateTimeOffset.Now.AddHours(-1), IsFavorite = true },
        new() { Id = "vs", Name = "Visual Studio", IconGlyph = "\uE943", Category = AppCategory.Development, LaunchCount = 88, LastUsed = DateTimeOffset.Now.AddDays(-1), IsFavorite = true },
        new() { Id = "terminal", Name = "Windows Terminal", IconGlyph = "\uE756", Category = AppCategory.Development, LaunchCount = 156, LastUsed = DateTimeOffset.Now.AddMinutes(-30), IsFavorite = true },
        new() { Id = "git", Name = "Git", IconGlyph = "\uE7A8", Category = AppCategory.Development, LaunchCount = 42, LastUsed = DateTimeOffset.Now.AddDays(-2) },
        new() { Id = "docker", Name = "Docker Desktop", IconGlyph = "\uE9F9", Category = AppCategory.Development, LaunchCount = 65, LastUsed = DateTimeOffset.Now.AddHours(-5) },

        // Design
        new() { Id = "figma", Name = "Figma", IconGlyph = "\uE771", Category = AppCategory.Design, LaunchCount = 110, LastUsed = DateTimeOffset.Now.AddHours(-3), IsFavorite = true },
        new() { Id = "photoshop", Name = "Photoshop", IconGlyph = "\uEB9C", Category = AppCategory.Design, LaunchCount = 38, LastUsed = DateTimeOffset.Now.AddDays(-3) },
        new() { Id = "snipaste", Name = "Snipaste", IconGlyph = "\uE8A9", Category = AppCategory.Design, LaunchCount = 72, LastUsed = DateTimeOffset.Now.AddHours(-8) },

        // Productivity
        new() { Id = "office", Name = "Office", IconGlyph = "\uE8B7", Category = AppCategory.Productivity, LaunchCount = 95, LastUsed = DateTimeOffset.Now.AddHours(-2), IsFavorite = true },
        new() { Id = "onedrive", Name = "OneDrive", IconGlyph = "\uE753", Category = AppCategory.Productivity, LaunchCount = 200, LastUsed = DateTimeOffset.Now.AddMinutes(-10) },
        new() { Id = "notion", Name = "Notion", IconGlyph = "\uE8A5", Category = AppCategory.Productivity, LaunchCount = 60, LastUsed = DateTimeOffset.Now.AddDays(-1) },
        new() { Id = "todo", Name = "Microsoft To Do", IconGlyph = "\uE8DB", Category = AppCategory.Productivity, LaunchCount = 28, LastUsed = DateTimeOffset.Now.AddDays(-2) },

        // Communication
        new() { Id = "teams", Name = "Teams", IconGlyph = "\uE8BD", Category = AppCategory.Communication, LaunchCount = 180, LastUsed = DateTimeOffset.Now.AddMinutes(-5), IsFavorite = true },
        new() { Id = "outlook", Name = "Outlook", IconGlyph = "\uE715", Category = AppCategory.Communication, LaunchCount = 145, LastUsed = DateTimeOffset.Now.AddHours(-1) },
        new() { Id = "wechat", Name = "WeChat", IconGlyph = "\uE8BD", Category = AppCategory.Communication, LaunchCount = 210, LastUsed = DateTimeOffset.Now.AddMinutes(-2), IsFavorite = true },

        // Entertainment
        new() { Id = "spotify", Name = "Spotify", IconGlyph = "\uE8F1", Category = AppCategory.Entertainment, LaunchCount = 130, LastUsed = DateTimeOffset.Now.AddHours(-4), IsFavorite = true },
        new() { Id = "netflix", Name = "Netflix", IconGlyph = "\uE8F2", Category = AppCategory.Entertainment, LaunchCount = 22, LastUsed = DateTimeOffset.Now.AddDays(-5) },
        new() { Id = "steam", Name = "Steam", IconGlyph = "\uE7FC", Category = AppCategory.Entertainment, LaunchCount = 55, LastUsed = DateTimeOffset.Now.AddDays(-1) },

        // System
        new() { Id = "explorer", Name = "文件资源管理器", IconGlyph = "\uEC50", Category = AppCategory.System, LaunchCount = 400, LastUsed = DateTimeOffset.Now.AddMinutes(-1), IsFavorite = true },
        new() { Id = "settings", Name = "设置", IconGlyph = "\uE713", Category = AppCategory.System, LaunchCount = 70, LastUsed = DateTimeOffset.Now.AddHours(-6) },
        new() { Id = "calc", Name = "计算器", IconGlyph = "\uE1D0", Category = AppCategory.System, LaunchCount = 35, LastUsed = DateTimeOffset.Now.AddDays(-1) },
        new() { Id = "snipping", Name = "截图工具", IconGlyph = "\uE8A9", Category = AppCategory.System, LaunchCount = 48, LastUsed = DateTimeOffset.Now.AddHours(-3) },

        // Other
        new() { Id = "edge", Name = "Microsoft Edge", IconGlyph = "\uE77B", Category = AppCategory.Other, LaunchCount = 350, LastUsed = DateTimeOffset.Now.AddMinutes(-3), IsFavorite = true },
        new() { Id = "chrome", Name = "Google Chrome", IconGlyph = "\uE77B", Category = AppCategory.Other, LaunchCount = 280, LastUsed = DateTimeOffset.Now.AddMinutes(-8), IsFavorite = true },
        new() { Id = "store", Name = "Microsoft Store", IconGlyph = "\uE719", Category = AppCategory.Other, LaunchCount = 18, LastUsed = DateTimeOffset.Now.AddDays(-4) },
    };

    /// <summary>快捷键配置列表。</summary>
    public static IReadOnlyList<HotkeyConfig> Hotkeys { get; } = new List<HotkeyConfig>
    {
        new() { DisplayKey = "Alt+Space", Action = HotkeyAction.ShowPanel },
        new() { DisplayKey = "Esc", Action = HotkeyAction.HidePanel },
        new() { DisplayKey = "Alt+1", Action = HotkeyAction.LaunchApp, AppName = "Visual Studio Code", AppIconGlyph = "\uE943" },
        new() { DisplayKey = "Alt+2", Action = HotkeyAction.LaunchApp, AppName = "Microsoft Edge", AppIconGlyph = "\uE77B" },
        new() { DisplayKey = "Alt+3", Action = HotkeyAction.LaunchApp, AppName = "Windows Terminal", AppIconGlyph = "\uE756" },
        new() { DisplayKey = "Alt+4", Action = HotkeyAction.LaunchApp, AppName = "文件资源管理器", AppIconGlyph = "\uEC50" },
        new() { DisplayKey = "Ctrl+Shift+D", Action = HotkeyAction.SwitchCategory },
    };

    /// <summary>默认设置。</summary>
    public static SettingsModel Settings { get; } = new()
    {
        Theme = ThemeMode.System,
        PanelOpacity = 0.85,
        AnimationsEnabled = true,
        TriggerHotkey = "Alt+Space",
        StartWithWindows = false,
        MinimizeToTray = true,
        ShowInTaskbar = false,
        AppVersion = "1.0.0.0 (原型)",
        EdgeGestures = new()
        {
            new() { Edge = ScreenEdge.Left, Enabled = true, Threshold = 10, Action = GestureAction.SwitchToFavorites },
            new() { Edge = ScreenEdge.Right, Enabled = true, Threshold = 10, Action = GestureAction.SwitchToRecent },
            new() { Edge = ScreenEdge.Top, Enabled = false, Threshold = 10, Action = GestureAction.OpenPanel },
            new() { Edge = ScreenEdge.Bottom, Enabled = false, Threshold = 10, Action = GestureAction.ClosePanel },
        },
    };

    /// <summary>获取指定分类的应用（收藏/最近为特殊分类）。</summary>
    public static IEnumerable<AppInfo> GetByCategory(AppCategory category)
    {
        return category switch
        {
            AppCategory.All => Apps,
            AppCategory.Favorites => Apps.Where(a => a.IsFavorite),
            AppCategory.Recent => Apps.OrderByDescending(a => a.LastUsed).Take(8),
            _ => Apps.Where(a => a.Category == category),
        };
    }
}
