using Godot;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Sinks.Godot.Interfaces;

namespace Serilog.Sinks.Godot
{
    public class GodotSink(ITextFormatter formatter, IGameConsole? gameConsole) : ILogEventSink
    {
        public const string SerilogPrefix = "[Serilog] ";
        public const string GodotPrefix = "[Godot] ";

        private readonly ITextFormatter _formatter = formatter;
        private readonly IGameConsole? _gameConsole = gameConsole;

        public void Emit(LogEvent logEvent)
        {
            using var buffer = new StringWriter();

            _formatter.Format(logEvent, buffer);

            string message = buffer.ToString().Trim();

            if (message.StartsWith(SerilogPrefix))
                return;

            switch (logEvent.Level)
            {
                case LogEventLevel.Verbose:
                    GD.PrintRich(SerilogPrefix + message);
                    _gameConsole?.AddLog(message);
                    break;
                case LogEventLevel.Debug:
                case LogEventLevel.Information:
                    GD.Print(SerilogPrefix + message);
                    _gameConsole?.AddLog(message);
                    break;
                case LogEventLevel.Warning:
                    if (logEvent.Exception == null)
                    {
                        GD.PrintRich($"{SerilogPrefix}[color=GOLD]{message}[/color]");
                        _gameConsole?.AddLog($"[color=GOLD]{message}[/color]");
                        return;
                    }
                    message = $"{message} => {logEvent.Exception.GetType().FullName}: {logEvent.Exception.Message}\n{logEvent.Exception.StackTrace}";
                    GD.PushWarning(SerilogPrefix + message);
                    _gameConsole?.AddLog($"[color=GOLD]{message}[/color]");
                    break;
                case LogEventLevel.Error:
                case LogEventLevel.Fatal:
                    if (logEvent.Exception == null)
                    {
                        GD.PrintErr(SerilogPrefix + message);
                        _gameConsole?.AddLog($"[color=RED]{message}[/color]");
                        return;
                    }
                    message = $"{message} => {logEvent.Exception.GetType().FullName}: {logEvent.Exception.Message}\n{logEvent.Exception.StackTrace}";
                    GD.PushError(SerilogPrefix + message);
                    _gameConsole?.AddLog($"[color=RED]{message}[/color]");
                    break;
            }
        }
    }
}
