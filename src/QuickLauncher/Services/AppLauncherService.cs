using System.Diagnostics;
using QuickLauncher.Models;

namespace QuickLauncher.Services;

/// <summary>
/// 基于 System.Diagnostics.Process 的应用启动服务。
/// </summary>
public sealed class AppLauncherService : IAppLauncherService
{
    public event EventHandler<AppLaunchedEventArgs>? AppLaunched;

    public async Task LaunchAsync(AppInfo app)
    {
        // 原型阶段 app.ExecutablePath 可能为空，回退到 explorer 打开
        if (!string.IsNullOrEmpty(app.ExecutablePath))
        {
            await LaunchByPathAsync(app.ExecutablePath, app.Args);
        }
        else
        {
            // 用 explorer 启动（按名称）
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "explorer.exe",
                    Arguments = $"shell:AppsFolder\\{app.Id}",
                    UseShellExecute = true,
                };
                Process.Start(psi);
                app.LaunchCount++;
                app.LastUsed = DateTimeOffset.Now;
                AppLaunched?.Invoke(this, new AppLaunchedEventArgs { App = app, Success = true });
            }
            catch (Exception ex)
            {
                AppLaunched?.Invoke(this, new AppLaunchedEventArgs { App = app, Success = false, ErrorMessage = ex.Message });
            }
        }
    }

    public async Task LaunchByPathAsync(string path, string[]? args = null)
    {
        await Task.Run(() =>
        {
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = path,
                    UseShellExecute = true,
                };
                if (args is { Length: > 0 })
                {
                    foreach (var a in args) psi.ArgumentList.Add(a);
                }
                Process.Start(psi);
                AppLaunched?.Invoke(this, new AppLaunchedEventArgs { Success = true });
            }
            catch (Exception ex)
            {
                AppLaunched?.Invoke(this, new AppLaunchedEventArgs { Success = false, ErrorMessage = ex.Message });
            }
        });
    }
}
