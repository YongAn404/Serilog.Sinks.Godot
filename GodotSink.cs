using Godot;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Sinks.Godot.Interfaces;

namespace Serilog.Sinks.Godot
{
    public class GodotSink(ITextFormatter formatter, IGameConsole? gameConsole) : ILogEventSink
    {
        private const string Prefix = "Serilog|";
        private readonly ITextFormatter _formatter = formatter;
        private readonly IGameConsole? _gameConsole = gameConsole;

        public void Emit(LogEvent logEvent)
        {
            using var buffer = new StringWriter();

            _formatter.Format(logEvent, buffer);

            string message = buffer.ToString().Trim();

            switch (logEvent.Level)
            {
                case LogEventLevel.Verbose:
                    GD.PrintRich(Prefix + message);
                    _gameConsole?.AddLog(message);
                    break;
                case LogEventLevel.Debug:
                case LogEventLevel.Information:
                    GD.Print(Prefix + message);
                    _gameConsole?.AddLog(message);
                    break;
                case LogEventLevel.Warning:
                    if (logEvent.Exception == null)
                    {
                        GD.PrintRich($"{Prefix}[color=GOLD]{message}[/color]");
                        _gameConsole?.AddLog($"[color=GOLD]{message}[/color]");
                        return;
                    }
                    message = $"{message} => {logEvent.Exception.GetType().FullName}: {logEvent.Exception.Message}\n{logEvent.Exception.StackTrace}";
                    GD.PushWarning(Prefix + message);
                    _gameConsole?.AddLog($"[color=GOLD]{message}[/color]");
                    break;
                case LogEventLevel.Error:
                case LogEventLevel.Fatal:
                    if (logEvent.Exception == null)
                    {
                        GD.PrintErr(Prefix + message);
                        _gameConsole?.AddLog($"[color=RED]{message}[/color]");
                        return;
                    }
                    message = $"{message} => {logEvent.Exception.GetType().FullName}: {logEvent.Exception.Message}\n{logEvent.Exception.StackTrace}";
                    GD.PushError(Prefix + message);
                    _gameConsole?.AddLog($"[color=RED]{message}[/color]");
                    break;
            }
        }
    }
}
