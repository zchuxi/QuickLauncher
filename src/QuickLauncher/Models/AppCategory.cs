namespace QuickLauncher.Models;

/// <summary>
/// 应用分类枚举。
/// </summary>
public enum AppCategory
{
    /// <summary>全部（仅用于筛选）</summary>
    All,

    /// <summary>收藏</summary>
    Favorites,

    /// <summary>最近使用</summary>
    Recent,

    /// <summary>开发工具</summary>
    Development,

    /// <summary>设计工具</summary>
    Design,

    /// <summary>效率办公</summary>
    Productivity,

    /// <summary>通讯社交</summary>
    Communication,

    /// <summary>娱乐</summary>
    Entertainment,

    /// <summary>系统工具</summary>
    System,

    /// <summary>其他</summary>
    Other,
}

/// <summary>
/// 分类扩展方法：获取显示名称与图标 glyph。
/// </summary>
public static class AppCategoryExtensions
{
    public static string GetDisplayName(this AppCategory category) => category switch
    {
        AppCategory.All => "全部",
        AppCategory.Favorites => "收藏",
        AppCategory.Recent => "最近",
        AppCategory.Development => "开发",
        AppCategory.Design => "设计",
        AppCategory.Productivity => "效率",
        AppCategory.Communication => "通讯",
        AppCategory.Entertainment => "娱乐",
        AppCategory.System => "系统",
        AppCategory.Other => "其他",
        _ => category.ToString(),
    };

    /// <summary>Segoe Fluent Icons 字符代码。</summary>
    public static string GetIconGlyph(this AppCategory category) => category switch
    {
        AppCategory.All => "\uE71D",
        AppCategory.Favorites => "\uE734",
        AppCategory.Recent => "\uE823",
        AppCategory.Development => "\uE943",
        AppCategory.Design => "\uE771",
        AppCategory.Productivity => "\uE8B7",
        AppCategory.Communication => "\uE8BD",
        AppCategory.Entertainment => "\uE8F1",
        AppCategory.System => "\uE7F4",
        AppCategory.Other => "\uE7B8",
        _ => "\uE7B8",
    };
}
