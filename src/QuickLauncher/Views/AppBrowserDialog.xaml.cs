using Microsoft.UI.Xaml.Controls;
using QuickLauncher.Models;
using QuickLauncher.ViewModels;

namespace QuickLauncher.Views;

/// <summary>
/// 应用浏览器对话框。
/// </summary>
public sealed partial class AppBrowserDialog : ContentDialog
{
    public AppBrowserViewModel ViewModel { get; }

    public AppBrowserDialog()
    {
        this.InitializeComponent();
        ViewModel = new AppBrowserViewModel();
    }

    /// <summary>分类标签切换。</summary>
    private void SelectorBar_SelectionChanged(SelectorBar sender, SelectorBarSelectionChangedEventArgs args)
    {
        if (sender.SelectedItem is SelectorBarItem item && item.Tag is string tagStr
            && System.Enum.TryParse<AppCategory>(tagStr, out var category))
        {
            ViewModel.SelectCategoryCommand.Execute(category);
        }
    }
}
