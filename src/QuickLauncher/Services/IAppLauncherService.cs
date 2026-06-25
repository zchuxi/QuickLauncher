using QuickLauncher.Models;

namespace QuickLauncher.Services;

/// <summary>
/// 应用启动服务接口。
/// </summary>
public interface IAppLauncherService
{
    Task LaunchAsync(AppInfo app);
    Task LaunchByPathAsync(string path, string[]? args = null);

    event EventHandler<AppLaunchedEventArgs>? AppLaunched;
}

public sealed class AppLaunchedEventArgs : EventArgs
{
    public AppInfo App { get; init; } = null!;
    public bool Success { get; init; }
    public string? ErrorMessage { get; init; }
}
