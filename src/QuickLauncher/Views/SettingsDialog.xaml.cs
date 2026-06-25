using Microsoft.UI.Xaml.Controls;
using QuickLauncher.Models;
using QuickLauncher.ViewModels;

namespace QuickLauncher.Views;

/// <summary>
/// 设置面板对话框。
/// </summary>
public sealed partial class SettingsDialog : ContentDialog
{
    public SettingsViewModel ViewModel { get; }

    public SettingsDialog()
    {
        this.InitializeComponent();
        ViewModel = new SettingsViewModel();
    }

    /// <summary>导航分组切换。</summary>
    private void Nav_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.SelectedItem is NavigationViewItem item && item.Tag is string section)
        {
            ViewModel.SelectSectionCommand.Execute(section);
        }
    }

    /// <summary>主题选择切换。</summary>
    private void ThemeSelector_SelectionChanged(SelectorBar sender, SelectorBarSelectionChangedEventArgs args)
    {
        if (sender.SelectedItem is SelectorBarItem item && item.Tag is string tagStr
            && System.Enum.TryParse<ThemeMode>(tagStr, out var theme))
        {
            ViewModel.Settings.Theme = theme;
        }
    }
}
