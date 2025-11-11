# Serilog.Sinks.Godot
## 安装教程
通过[NuGet](https://www.nuget.org/packages/Serilog.Sinks.Godot)安装Serilog.Sinks.Godot
## 在Godot使用Serilog教程
```cs
Log.Logger = new LoggerConfiguration()
    .WriteTo.Godot()
    .CreateLogger();
```
## 将Godot的Logger重定向到Serilog
！！！请不要重复添加！！！
```cs
OS.AddLogger(new GodotLogger());
```
## 在Godot安装Serilog.Sinks.File情况下
这个例子是将文件保存在游戏运行目录下 ``[不适用移动端]``
```cs
Log.Logger = new LoggerConfiguration()
    .WriteTo.Godot()
    #if !TOOLS
    .WriteTo.File("Logs/.log", rollingInterval: RollingInterval.Day)
    #endif
    .CreateLogger();
```