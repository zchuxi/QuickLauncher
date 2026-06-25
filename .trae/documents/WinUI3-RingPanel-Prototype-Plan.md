# WinUI 3 环形启动器 - 页面原型开发计划

## 摘要

基于 WinUI 3 (Windows App SDK) 开发一款环形应用启动器，采用 Windows 默认 Fluent Design 风格（标准 Mica 材质 + 系统强调色 + Segoe UI 字体，不使用自定义霓虹主题）。本阶段聚焦**软件页面原型开发**，一次性实现全部 4 个核心页面（主面板、应用浏览器、快捷键设置、设置面板），使用 Mock 数据可运行预览，暂不接入系统功能（全局快捷键、鼠标钩子、应用扫描等留待后续阶段）。

## 当前状态分析

- 工作区 `/workspace` 仅有 `.trae/documents/` 下的 PRD.md 和 Technical-Architecture.md 两份规划文档
- **无任何代码文件**（已通过 Glob 确认无 .cs/.xaml/.csproj/.sln）
- 现有文档中的设计风格为"霓虹蓝/紫"自定义主题，需调整为 **Windows 默认 Fluent 风格**（用户明确要求）
- 现有文档中主面板为"弧形"，需调整为**环形轨迹 + 中心分类切换器**布局（用户确认）

## 关键设计决策

### 1. 设计风格（Windows 默认 Fluent）
- **材质**: Mica Backdrop (主面板/设置) + Acrylic (弹窗)
- **配色**: 完全使用系统主题资源（`{ThemeResource ...}`），不硬编码自定义颜色
  - 强调色: `{ThemeResource AccentFillColorDefaultBrush}`（跟随系统设置）
  - 背景: `{ThemeResource LayerFillColorDefaultBrush}` / `{ThemeResource CardBackgroundFillColorDefaultBrush}`
  - 文字: `{ThemeResource TextFillColorPrimaryBrush}` 等
- **字体**: Segoe UI Variable（系统默认，不引入额外字体）
- **圆角**: 控件 4-8px，面板 16px（遵循 WinUI 3 默认 ControlStrokeThickness/CornerRadius）
- **图标**: 使用 Segoe Fluent Icons 字体（系统内置）

### 2. 环形面板布局（环形轨迹 + 中心分类）
```
        ┌──App──App──App──┐
      App                   App
     App    ┌─────────┐    App
     App    │ 分类切换 │    App   ← 中心圆环（滚轮切换分类）
     App    │  收藏   │    App      中心点显示当前分类名
      App   └─────────┘   App
        └──App──App──App──┘
              [搜索框]       ← 底部搜索
```
- 应用图标沿**外环轨迹**等距排列（最多 8-10 个）
- **中心圆环**作为分类切换器：鼠标滚轮旋转切换分类
- **中心点**显示当前分类名称
- 底部 AutoSuggestBox 搜索框

### 3. 原型范围（4 个页面 + Mock 数据）
所有页面使用 Mock 数据可运行预览，不接入真实系统功能。

## 实施步骤

### 步骤 1: 项目骨架搭建
创建 WinUI 3 项目基础文件：

**文件列表:**
- `/workspace/QuickLauncher.sln` - 解决方案文件
- `/workspace/src/QuickLauncher/QuickLauncher.csproj` - 项目文件（net8.0-windows10.0.19041.0, WinUI 3, CommunityToolkit.Mvvm, unpackaged 模式便于原型运行）
- `/workspace/src/QuickLauncher/app.manifest` - 应用清单（DPI 感知）
- `/workspace/src/QuickLauncher/App.xaml` - 应用资源（合并 XamlControlsResources + 自定义 Themes）
- `/workspace/src/QuickLauncher/App.xaml.cs` - App 入口（初始化主窗口）
- `/workspace/src/QuickLauncher/Program.cs` - Main 入口（简化版，暂不做单实例）

**为什么:** WinUI 3 项目必须的基础骨架。采用 unpackaged（`WindowsPackageType=None`）模式，便于原型快速运行调试，无需 MSIX 打包。

### 步骤 2: 数据模型与 Mock 数据
**文件列表:**
- `/workspace/src/QuickLauncher/Models/AppCategory.cs` - 应用分类枚举（Favorites/Recent/Development/Design/Productivity/Communication/Entertainment/System/Other）
- `/workspace/src/QuickLauncher/Models/AppInfo.cs` - 应用信息 record（Id/Name/IconGlyph/Category/LaunchCount/LastUsed/IsFavorite）
- `/workspace/src/QuickLauncher/Models/HotkeyConfig.cs` - 快捷键配置 record
- `/workspace/src/QuickLauncher/Models/SettingsModel.cs` - 设置模型（Theme/PanelOpacity/AnimationsEnabled/EdgeGestures 等）
- `/workspace/src/QuickLauncher/Data/MockData.cs` - 静态 Mock 数据（20+ 应用、若干快捷键、默认设置）

**为什么:** 原型需要数据展示。IconGlyph 用 Segoe Fluent Icons 字符代码（如 `\uE71B` 文件夹），无需真实应用图标文件。

**Mock 数据要点:**
- 应用按分类分组，每分类 3-6 个
- 使用 Segoe Fluent Icons glyph 作为图标（避免外部图片资源）
- 快捷键示例：Alt+Space→显示面板, Alt+1→VSCode, Alt+2→Chrome

### 步骤 3: 主题资源与样式
**文件列表:**
- `/workspace/src/QuickLauncher/Themes/Styles.xaml` - 自定义样式（AppIconButtonStyle/RingPanelStyle 等），全部基于系统 ThemeResource

**为什么:** 确保 Windows 默认风格，不引入自定义霓虹色。所有颜色引用 `{ThemeResource ...}`，自动跟随系统亮/暗主题。

### 步骤 4: 环形面板自定义控件
**文件列表:**
- `/workspace/src/QuickLauncher/Controls/RingPanel.cs` - 自定义 Panel 控件，继承 `Panel`，重写 `MeasureOverride`/`ArrangeOverride`
  - 将子元素沿环形轨迹等距排列
  - 支持属性：`Radius`（环半径）、`StartAngle`、`SweepAngle`
- `/workspace/src/QuickLauncher/Controls/CategoryRing.cs` - 中心分类切换器控件（圆环 + 中心文字），支持滚轮切换

**为什么:** WinUI 3 无内置环形布局控件，需自定义 Panel 实现图标沿圆周排列。中心分类切换器是环形面板的核心交互。

**RingPanel 算法:**
```csharp
// Arrange: 每个子元素角度 = StartAngle + i * (SweepAngle / count)
// 位置 = center + (radius * cos(angle), radius * sin(angle))
```

### 步骤 5: ViewModel 层（4 个）
**文件列表:**
- `/workspace/src/QuickLauncher/ViewModels/MainViewModel.cs` - 主面板 VM
  - `ObservableCollection<AppInfo> RingApps`（当前分类应用）
  - `ObservableCollection<AppCategory> Categories`
  - `AppCategory CurrentCategory`（滚轮切换）
  - `string SearchQuery`
  - `RelayCommand LaunchApp(AppInfo)`
  - `RelayCommand SwitchCategory(bool next)`
- `/workspace/src/QuickLauncher/ViewModels/AppBrowserViewModel.cs` - 应用浏览器 VM
  - `ObservableCollection<AppInfo> AllApps` / `FilteredApps`
  - `AppCategory SelectedCategory`
  - 搜索/分类过滤逻辑
- `/workspace/src/QuickLauncher/ViewModels/HotkeySettingsViewModel.cs` - 快捷键设置 VM
  - `ObservableCollection<HotkeyConfig> Hotkeys`
  - 增删改命令
- `/workspace/src/QuickLauncher/ViewModels/SettingsViewModel.cs` - 设置 VM
  - `SettingsModel Settings`
  - 各设置项绑定

**为什么:** CommunityToolkit.Mvvm 的 `ObservableObject`/`ObservableProperty`/`RelayCommand` source generator 简化 MVVM 代码。原型阶段命令仅更新 Mock 数据状态。

### 步骤 6: 主面板页面（环形面板）
**文件列表:**
- `/workspace/src/QuickLauncher/Views/MainPanel.xaml` - 主面板 UI
- `/workspace/src/QuickLauncher/Views/MainPanel.xaml.cs` - 代码后置（滚轮事件处理）

**XAML 结构:**
```xml
<Window>
  <Grid>
    <Grid.Background><MicaBackdrop/></Grid.Background>
    <Border CornerRadius="16" Width="600" Height="600">
      <Grid>
        <!-- 外环：RingPanel 放 AppIconButton -->
        <controls:RingPanel Radius="220">
          <Button Style="AppIconButtonStyle"/> ... ×N
        </controls:RingPanel>
        
        <!-- 中心：CategoryRing 分类切换器 -->
        <controls:CategoryRing Width="160" Height="160"
                              Category="{x:Bind VM.CurrentCategory, Mode=TwoWay}"/>
        
        <!-- 底部搜索框 -->
        <AutoSuggestBox PlaceholderText="搜索应用..." VerticalAlignment="Bottom"/>
      </Grid>
    </Border>
  </Grid>
</Window>
```

**交互（原型级）:**
- 鼠标滚轮在中心圆环上 → 切换分类，外环图标更新
- 点击外环图标 → 显示启动 Toast 通知（模拟启动）
- 搜索框输入 → 过滤外环图标

### 步骤 7: 应用浏览器页面
**文件列表:**
- `/workspace/src/QuickLauncher/Views/AppBrowserDialog.xaml` + `.cs`

**XAML 结构:**
- `ContentDialog` 模态弹窗，Acrylic 背景
- 顶部 `AutoSuggestBox` 搜索
- `SelectorBar` 分类标签（全部/开发/设计/效率/通讯/娱乐/系统）
- `ScrollViewer` + `ItemsRepeater` + `UniformGridLayout` 应用卡片网格
- 每张卡片：图标 + 名称 + 分类 + 使用次数
- 底部 PrimaryButton "选择" / CloseButton "取消"

### 步骤 8: 快捷键设置页面
**文件列表:**
- `/workspace/src/QuickLauncher/Views/HotkeySettingsDialog.xaml` + `.cs`

**XAML 结构:**
- `ContentDialog` 模态弹窗
- `ListView` 显示已绑定快捷键：左侧快捷键徽章（`Border` + `TextBlock`，Cascadia Code 字体）+ 箭头 + 右侧应用图标+名称
- 行 hover 显示编辑/删除图标按钮
- 底部 "+ 添加快捷键" 按钮
- 添加时展开录入区（快捷键录制 TextBox + 应用选择 ComboBox）

### 步骤 9: 设置面板页面
**文件列表:**
- `/workspace/src/QuickLauncher/Views/SettingsDialog.xaml` + `.cs`

**XAML 结构:**
- `ContentDialog` 模态弹窗
- `NavigationView` 左侧导航：外观 / 快捷键 / 鼠标手势 / 高级 / 关于
- 右侧 `Frame`/`ContentControl` 显示对应分组
- **外观**: 主题 `SelectorBar`（浅色/深色/跟随系统）+ 透明度 `Slider` + 动画 `ToggleSwitch`
- **快捷键**: 触发键显示 + "更改"按钮
- **鼠标手势**: 四边缘 `ToggleSwitch` + 阈值 `Slider` + 动作 `ComboBox`
- **高级**: 开机启动 `ToggleSwitch` + 最小化到托盘 `ToggleSwitch`
- **关于**: 版本信息

### 步骤 10: 应用入口整合与主窗口导航
**修改文件:**
- `/workspace/src/QuickLauncher/App.xaml.cs` - 初始化 `MainWindow`，注入 ViewModel
- 新增 `/workspace/src/QuickLauncher/Views/MainWindow.xaml` + `.cs` - 主窗口，包含导航按钮打开各弹窗
  - 顶部一个简单标题栏 + 4 个按钮（主面板/应用浏览器/快捷键/设置）
  - 原型阶段用主窗口作为入口，点击按钮打开对应 ContentDialog

**为什么:** WinUI 3 主面板窗口通常无边框透明，但原型阶段需要一个可导航入口。用 MainWindow 承载导航按钮，便于预览所有页面。

## 假设与决策

1. **unpackaged 模式**: 原型用 `WindowsPackageType=None`，直接运行 exe，无需 MSIX 打包。后续正式版可切回 packaged。
2. **Segoe Fluent Icons**: 使用系统内置图标字体，无需引入图片资源。若系统为 Win10 可能缺该字体，回退到 Segoe MDL2 Assets。
3. **Mock 数据不持久化**: 原型阶段设置变更仅内存生效，关闭即失。后续接 `SettingsService` 持久化。
4. **无真实应用启动**: 点击应用图标显示 `InfoBar` 或 Toast 提示"已启动 XXX（原型）"，不调用 Process.Start。
5. **窗口样式**: 原型主窗口为标准带标题栏窗口（便于调试），正式版主面板改为无边框透明圆形窗口。
6. **目标框架**: net8.0-windows10.0.19041.0，最低 17763，WinUI 3 1.5+。

## 验证步骤

1. **构建验证**: `dotnet build` 成功无错误
2. **运行验证**: `dotnet run` 启动主窗口，显示导航按钮
3. **页面验证**（逐一点击导航按钮）:
   - 主面板: 显示环形布局，外环有应用图标，中心分类切换器，滚轮可切换分类，外环图标随之更新
   - 应用浏览器: 显示分类标签 + 应用卡片网格，搜索框可过滤，分类标签可切换
   - 快捷键设置: 显示快捷键列表，可点添加展开录入区
   - 设置面板: NavigationView 可切换分组，各设置控件正常显示交互
4. **主题验证**: 切换 Windows 系统亮/暗主题，应用自动跟随（验证未硬编码颜色）
5. **强调色验证**: 更改 Windows 系统强调色，应用按钮/选中态跟随变化

## 文件清单总计

| 类别 | 文件数 | 说明 |
|------|--------|------|
| 项目骨架 | 6 | sln/csproj/manifest/App.xaml/App.xaml.cs/Program.cs |
| Models | 4 | AppCategory/AppInfo/HotkeyConfig/SettingsModel |
| Data | 1 | MockData |
| Themes | 1 | Styles |
| Controls | 2 | RingPanel/CategoryRing |
| ViewModels | 4 | Main/AppBrowser/HotkeySettings/Settings |
| Views | 10 | MainWindow + MainPanel + 3 Dialog (xaml+cs) |
| **合计** | **28** | |

## 后续阶段（本计划不含）

- 阶段 2: 服务层实现（HotkeyService/MouseHookService/AppScannerService/AppLauncherService/SettingsService）
- 阶段 3: 系统集成（全局快捷键注册、鼠标钩子、应用扫描、开机启动、系统托盘）
- 阶段 4: 数据持久化（electron-store 等价 → ApplicationData JSON）
- 阶段 5: 打包发布（MSIX）
