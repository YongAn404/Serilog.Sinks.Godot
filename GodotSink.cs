using Godot;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Serilog.Sinks.Godot
{
    public class GodotSink(ITextFormatter formatter) : ILogEventSink
    {
        private readonly ITextFormatter formatter = formatter;

        public void Emit(LogEvent logEvent)
        {
            using var buffer = new StringWriter();

            formatter.Format(logEvent, buffer);

            string message = buffer.ToString().Trim();

            switch (logEvent.Level)
            {
                case LogEventLevel.Verbose:
                    GD.PrintRich(message);
                    break;
                case LogEventLevel.Debug:
                case LogEventLevel.Information:
                    GD.Print(message);
                    break;
                case LogEventLevel.Warning:
                    if (logEvent.Exception == null)
                    {
                        GD.PrintRich($"[color=GOLD]{message}[/color]");
                        return;
                    }
                    GD.PushWarning($"{message}::{logEvent.Exception.Message}");
                    break;
                case LogEventLevel.Error:
                case LogEventLevel.Fatal:
                    if (logEvent.Exception == null)
                    {
                        GD.PrintErr(message);
                        return;
                    }
                    GD.PushError($"{message}::{logEvent.Exception.Message}");
                    break;
            }
        }
    }
}
