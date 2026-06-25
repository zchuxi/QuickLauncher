using Microsoft.Win32;

namespace QuickLauncher.Services;

/// <summary>
/// 开机启动服务：通过注册表 HKCU\Software\Microsoft\Windows\CurrentVersion\Run 实现。
/// </summary>
public sealed class StartupService : IStartupService
{
    private const string RunKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
    private const string ValueName = "QuickLauncher";

    private static string AppExePath =>
        System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName
        ?? Environment.ProcessPath
        ?? string.Empty;

    public bool IsEnabled()
    {
        using var key = Registry.CurrentUser.OpenSubKey(RunKeyPath);
        return key?.GetValue(ValueName) is not null;
    }

    public void Enable()
    {
        using var key = Registry.CurrentUser.CreateSubKey(RunKeyPath, true);
        var path = AppExePath;
        if (!string.IsNullOrEmpty(path))
        {
            key.SetValue(ValueName, $"\"{path}\"");
        }
    }

    public void Disable()
    {
        using var key = Registry.CurrentUser.CreateSubKey(RunKeyPath, true);
        if (key.GetValue(ValueName) is not null)
        {
            key.DeleteValue(ValueName, false);
        }
    }
}
