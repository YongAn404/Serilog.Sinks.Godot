# Serilog.Sinks.Godot
## 在Godot使用Serilog教程
```cs
Log.Logger = new LoggerConfiguration()
    .WriteTo.Godot()
    .CreateLogger();
```
## 在Godot安装Serilog.Sinks.File情况下
这个例子是将文件保存在游戏运行目录下 ``[不适用移动端]``
```cs
Log.Logger = new LoggerConfiguration()
    .WriteTo.Godot()
    #if !TOOLS
    .WriteTo.File("Logs/.txt", rollingInterval: RollingInterval.Minute)
    #endif
    .CreateLogger();
```