using QuickLauncher.Models;

namespace QuickLauncher.Services;

/// <summary>
/// 应用扫描服务接口：扫描已安装应用。
/// </summary>
public interface IAppScannerService
{
    Task<IReadOnlyList<AppInfo>> ScanInstalledAppsAsync(IProgress<double>? progress = null);
    IReadOnlyList<AppInfo> GetCached();
}
