using System.Diagnostics;
using Microsoft.Win32;
using QuickLauncher.Models;

namespace QuickLauncher.Services;

/// <summary>
/// 扫描已安装应用：从注册表（Uninstall 键）+ 开始菜单快捷方式。
/// 原型阶段回退到 Mock 数据。
/// </summary>
public sealed class AppScannerService : IAppScannerService
{
    private List<AppInfo> _cache = new();

    public IReadOnlyList<AppInfo> GetCached() => _cache;

    public async Task<IReadOnlyList<AppInfo>> ScanInstalledAppsAsync(IProgress<double>? progress = null)
    {
        var result = new List<AppInfo>();
        var paths = new[]
        {
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
            @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall",
        };

        await Task.Run(() =>
        {
            int total = paths.Length;
            for (int i = 0; i < total; i++)
            {
                using var key = Registry.LocalMachine.OpenSubKey(paths[i]);
                if (key is null) continue;

                foreach (var sub in key.GetSubKeyNames())
                {
                    using var sk = key.OpenSubKey(sub);
                    if (sk is null) continue;

                    var name = sk.GetValue("DisplayName") as string;
                    var loc = sk.GetValue("DisplayIcon") as string ?? sk.GetValue("InstallLocation") as string;
                    if (string.IsNullOrEmpty(name)) continue;

                    result.Add(new AppInfo
                    {
                        Id = sub,
                        Name = name,
                        ExecutablePath = loc ?? string.Empty,
                        IconGlyph = "\uE7B8",
                        Category = AppCategory.Other,
                    });
                }

                progress?.Report((i + 1) * 100.0 / total);
            }
        });

        // 无扫描结果时回退到 Mock
        if (result.Count == 0)
        {
            result = Data.MockData.Apps.ToList();
        }

        _cache = result;
        return result;
    }
}
