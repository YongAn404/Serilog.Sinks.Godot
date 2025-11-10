using Godot;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Sinks.Godot.Interfaces;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Serilog.Sinks.Godot
{
    public class GodotSink(ITextFormatter formatter, IGameConsole? gameConsole) : ILogEventSink
    {
        private const string Prefix = "Serilog|";
        private readonly ITextFormatter formatter = formatter;
        private readonly IGameConsole? gameConsole = gameConsole;

        public void Emit(LogEvent logEvent)
        {
            using var buffer = new StringWriter();

            formatter.Format(logEvent, buffer);

            string message = buffer.ToString().Trim();

            switch (logEvent.Level)
            {
                case LogEventLevel.Verbose:
                    GD.PrintRich(Prefix + message);
                    gameConsole?.AddLog(message);
                    break;
                case LogEventLevel.Debug:
                case LogEventLevel.Information:
                    GD.Print(Prefix + message);
                    gameConsole?.AddLog(message);
                    break;
                case LogEventLevel.Warning:
                    if (logEvent.Exception == null)
                    {
                        GD.PrintRich($"[color=GOLD]{Prefix + message}[/color]");
                        gameConsole?.AddLog($"[color=GOLD]{message}[/color]");
                        return;
                    }
                    message = $"{message} => {logEvent.Exception.GetType().FullName}: {logEvent.Exception.Message}\n{logEvent.Exception.StackTrace}";
                    GD.PushWarning(Prefix + message);
                    gameConsole?.AddLog($"[color=GOLD]{message}[/color]");
                    break;
                case LogEventLevel.Error:
                case LogEventLevel.Fatal:
                    if (logEvent.Exception == null)
                    {
                        GD.PrintErr(Prefix + message);
                        gameConsole?.AddLog($"[color=RED]{message}[/color]");
                        return;
                    }
                    message = $"{message} => {logEvent.Exception.GetType().FullName}: {logEvent.Exception.Message}\n{logEvent.Exception.StackTrace}";
                    GD.PushError(Prefix + message);
                    gameConsole?.AddLog($"[color=RED]{message}[/color]");
                    break;
            }
        }
    }
}
