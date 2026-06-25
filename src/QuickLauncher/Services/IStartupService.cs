namespace QuickLauncher.Services;

/// <summary>
/// 开机启动服务接口。
/// </summary>
public interface IStartupService
{
    bool IsEnabled();
    void Enable();
    void Disable();
}
