# Serilog.Sinks.Godot

Serilog.Sinks.Godot 是一个专为 Godot 引擎设计的 Serilog 输出目标（Sink），它能够将日志信息输出到 Godot 的控制台。该库同时支持在 Godot 编辑器中运行以及游戏发布后的运行时使用，并且可以与其他 Serilog Sink 无缝集成。

## 特性

- 支持在 Godot 编辑器的控制台中输出 Serilog 日志
- 支持在游戏内置控制台（需实现 `IGameConsole` 接口）中输出日志
- 将 GDScript 或 Godot 引擎自身的错误日志转发到 Serilog（特殊处理，避免递归问题）
- 支持自定义 Godot 错误信息的输出格式

## 使用指南

通过 [NuGet](https://www.nuget.org/packages/Serilog.Sinks.Godot) 安装 `Serilog.Sinks.Godot` 包。

### 将日志输出到 Godot 控制台

```csharp
Log.Logger = new LoggerConfiguration()
    .WriteTo.Godot()
    .CreateLogger();
```

### 同时输出到 Godot 控制台与游戏内置控制台

```csharp
public partial class Console : Panel, IGameConsole
{
    // ...

    public Console()
    {
        // ...

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Godot(this)   // 传递实现了 IGameConsole 的实例
            .CreateLogger();

        OS.AddLogger(new GodotLogger());
    }

    // ...
}
```

### 将 Godot 日志重定向到 Serilog

> **注意**：请勿重复添加 `GodotLogger`！

```csharp
OS.AddLogger(new GodotLogger());
```

#### 自定义 Godot 错误输出格式

可以通过构造函数指定自定义的输出模板：

```csharp
OS.AddLogger(new GodotLogger("({ErrorType}:{EditorNotify}) {File}:{Function} {Line} => \"{Rationale}\"\n" +
            "{Code}\n" +
            "------Backtrace------" +
            "{ScriptBacktraces}" +
            "\n---------End---------\n"));
```

或通过属性设置：

```csharp
GodotLogger logger = new GodotLogger();
logger.GodotErrorOutputTemplate = "({ErrorType}:{EditorNotify}) {File}:{Function} {Line} => \"{Rationale}\"\n" +
            "{Code}\n" +
            "------Backtrace------" +
            "{ScriptBacktraces}" +
            "\n---------End---------\n";
OS.AddLogger(logger);
```

### 与 Serilog.Sinks.File 结合使用

以下示例将日志同时输出到 Godot 控制台和文件（文件保存在游戏运行目录下，**不适用于移动端**）：

```csharp
Log.Logger = new LoggerConfiguration()
    .WriteTo.Godot()
#if !TOOLS
    .WriteTo.File("Logs/.log", rollingInterval: RollingInterval.Day)
#endif
    .CreateLogger();
```