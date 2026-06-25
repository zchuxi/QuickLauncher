# QuickLauncher - 鼠标极速启动器

一款基于 **WinUI 3** 的 Windows 应用快速启动与切换工具，通过全局快捷键和鼠标边缘手势呼出**环形启动面板**，实现高效应用启动与分类切换。

## 核心特性

- **环形启动面板**：应用图标沿环形轨迹排列，中心圆环作为分类切换器，鼠标滚轮切换分类
- **全局快捷键**：`Alt+Space` 呼出面板，可自定义快捷键绑定应用
- **鼠标边缘手势**：光标移至屏幕边缘触发分类切换/面板开关
- **应用浏览器**：搜索、分类筛选、网格浏览全部应用
- **Windows Fluent 风格**：Mica 材质 + 系统强调色 + Segoe UI，自动跟随亮/暗主题
- **系统集成**：系统托盘、开机启动、单实例

## 技术栈

| 类别 | 技术 |
|------|------|
| 桌面框架 | WinUI 3 (Windows App SDK 1.5) |
| 运行时 | .NET 8 |
| 语言 | C# 12 / XAML |
| MVVM | CommunityToolkit.Mvvm |
| 系统托盘 | H.NotifyIcon.WinUI |
| 系统集成 | Win32 P/Invoke (RegisterHotKey / SetWindowsHookEx) |

## 项目结构

```
src/QuickLauncher/
├── Models/          # 数据模型 (AppInfo, HotkeyConfig, SettingsModel...)
├── Data/            # Mock 数据
├── Services/        # 服务层 (Hotkey/MouseHook/AppLauncher/AppScanner/Settings/Startup/Tray)
├── Helpers/         # Win32 封装 (NativeMethods, HotkeyParser)
├── Controls/        # 自定义控件 (RingPanel, CategoryRing)
├── ViewModels/      # MVVM 视图模型
├── Views/           # XAML 页面 (MainWindow/MainPanel + 3 Dialog)
├── Themes/          # 样式资源 (Styles, Generic)
└── Converters/      # 值转换器
```

## 快速开始

### 环境要求

- Windows 10 1809 (17763) 及以上
- Visual Studio 2022 (17.8+) 或 .NET 8 SDK
- Windows App SDK 工作负载

### 构建运行

```bash
git clone https://github.com/zchuxi/QuickLauncher.git
cd QuickLauncher
dotnet restore
dotnet build
dotnet run --project src/QuickLauncher/QuickLauncher.csproj
```

项目默认使用 **unpackaged 模式**（`WindowsPackageType=None`），可直接运行 exe，无需 MSIX 打包。

### 使用方式

1. 启动后显示主窗口，含四个导航按钮预览各页面
2. 按 `Alt+Space`（全局快捷键）呼出环形主面板
3. 在主面板：鼠标滚轮滚动中心圆环切换分类，点击外环图标启动应用
4. 底部搜索框实时过滤应用
5. 系统托盘图标可右键打开菜单

## 系统集成说明

| 功能 | 实现方式 |
|------|----------|
| 全局快捷键 | `user32.dll!RegisterHotKey` + 隐藏消息窗口接收 WM_HOTKEY |
| 鼠标手势 | `SetWindowsHookEx(WH_MOUSE_LL)` 全局低级鼠标钩子 |
| 应用启动 | `System.Diagnostics.Process` + `shell:AppsFolder` |
| 应用扫描 | 注册表 `HKLM\...\Uninstall` + 开始菜单 |
| 设置持久化 | JSON 文件 `%APPDATA%\QuickLauncher\settings.json` |
| 开机启动 | 注册表 `HKCU\...\Run` |
| 系统托盘 | H.NotifyIcon.WinUI |
| 主题跟随 | 全部使用 `{ThemeResource}` 资源，自动响应系统主题 |

## 开发说明

- 原型阶段部分数据使用 Mock 数据，可在 `Data/MockData.cs` 修改
- 自定义环形布局见 `Controls/RingPanel.cs`（三角函数定位子元素）
- 中心分类切换器见 `Controls/CategoryRing.cs`（处理滚轮事件）

## License

MIT
