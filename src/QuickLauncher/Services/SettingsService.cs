using System.IO;
using System.Text.Json;
using QuickLauncher.Models;

namespace QuickLauncher.Services;

/// <summary>
/// 设置服务：JSON 文件持久化。路径：%APPDATA%\QuickLauncher\settings.json
/// </summary>
public sealed class SettingsService : ISettingsService
{
    private static readonly string AppDir = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "QuickLauncher");
    private static readonly string FilePath = Path.Combine(AppDir, "settings.json");

    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public SettingsModel Current { get; private set; }

    public event EventHandler? SettingsChanged;

    public SettingsService()
    {
        Current = Load();
    }

    public void Save()
    {
        try
        {
            Directory.CreateDirectory(AppDir);
            var json = JsonSerializer.Serialize(Current, JsonOpts);
            File.WriteAllText(FilePath, json);
            SettingsChanged?.Invoke(this, EventArgs.Empty);
        }
        catch
        {
            // 持久化失败不阻塞 UI
        }
    }

    public void Reload()
    {
        Current = Load();
        SettingsChanged?.Invoke(this, EventArgs.Empty);
    }

    private static SettingsModel Load()
    {
        try
        {
            if (File.Exists(FilePath))
            {
                var json = File.ReadAllText(FilePath);
                var s = JsonSerializer.Deserialize<SettingsModel>(json, JsonOpts);
                if (s is not null) return s;
            }
        }
        catch
        {
            // 忽略，回退默认
        }
        return CloneDefault();
    }

    private static SettingsModel CloneDefault() => new()
    {
        Theme = Data.MockData.Settings.Theme,
        PanelOpacity = Data.MockData.Settings.PanelOpacity,
        AnimationsEnabled = Data.MockData.Settings.AnimationsEnabled,
        TriggerHotkey = Data.MockData.Settings.TriggerHotkey,
        StartWithWindows = Data.MockData.Settings.StartWithWindows,
        MinimizeToTray = Data.MockData.Settings.MinimizeToTray,
        ShowInTaskbar = Data.MockData.Settings.ShowInTaskbar,
        AppVersion = Data.MockData.Settings.AppVersion,
        EdgeGestures = Data.MockData.Settings.EdgeGestures
            .Select(e => new EdgeGestureSetting
            {
                Edge = e.Edge,
                Enabled = e.Enabled,
                Threshold = e.Threshold,
                Action = e.Action,
            }).ToList(),
    };
}
