using QuickLauncher.Services;

namespace QuickLauncher;

/// <summary>
/// 简易服务定位器。原型阶段使用单例，后续可替换为 DI 容器。
/// </summary>
public static class ServiceLocator
{
    public static IHotkeyService Hotkey { get; } = new HotkeyService();
    public static IMouseHookService MouseHook { get; } = new MouseHookService();
    public static IAppLauncherService Launcher { get; } = new AppLauncherService();
    public static IAppScannerService Scanner { get; } = new AppScannerService();
    public static ISettingsService Settings { get; } = new SettingsService();
    public static IStartupService Startup { get; } = new StartupService();
    public static ITrayService Tray { get; } = new TrayService();
}
