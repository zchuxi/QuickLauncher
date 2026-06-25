using System.Text.Json.Serialization;

namespace QuickLauncher.Models;

/// <summary>
/// 应用信息模型。
/// </summary>
public sealed class AppInfo
{
    public string Id { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;

    /// <summary>Segoe Fluent Icons 字符代码，用于原型显示。</summary>
    public string IconGlyph { get; init; } = "\uE7B8";

    public AppCategory Category { get; init; } = AppCategory.Other;

    /// <summary>启动次数。</summary>
    public int LaunchCount { get; set; }

    public DateTimeOffset LastUsed { get; set; } = DateTimeOffset.MinValue;

    public bool IsFavorite { get; set; }

    /// <summary>可执行文件路径（扫描注册表获得，可能为空）。</summary>
    public string ExecutablePath { get; init; } = string.Empty;

    /// <summary>启动参数。</summary>
    public string[] Args { get; init; } = Array.Empty<string>();

    /// <summary>显示用副标题。</summary>
    [JsonIgnore]
    public string Subtitle => $"{Category.GetDisplayName()} · {LaunchCount} 次";
}
