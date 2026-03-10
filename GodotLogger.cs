using Godot;
using Godot.Collections;

namespace Serilog.Sinks.Godot
{
    public partial class GodotLogger : Logger
    {
        public const string Prefix = "Serilog|";
        public string GodotErrorOutputTemplate { get; set; } =
            "({ErrorType}:{EditorNotify}) {File}:{Function} {Line} => \"{Rationale}\"\n" +
            "{Code}\n" +
            "------Backtrace------" +
            "{ScriptBacktraces}" +
            "\n---------End---------\n";

        public override void _LogError(string function, string? file, int line, string code, string? rationale, bool editorNotify, int errorType, Array<ScriptBacktrace> scriptBacktraces)
        {
            file ??= "文件无法捕获";
            rationale ??= "无进一步信息";
            string scriptBacktracesText = string.Join("\n", scriptBacktraces.Select((sb) => sb.Format()));
            Log.Error(GodotErrorOutputTemplate, (ErrorType)errorType, editorNotify, file, function, line, rationale, code, scriptBacktracesText);
        }

        public override void _LogMessage(string message, bool error)
        {
            if (message.StartsWith(Prefix) == true)
                return;

            message = "GODOT|" + message[..(message.Length - 1)];
            if (error)
            {
                Log.Error(message);
                return;
            }
            Log.Information(message);
        }
    }
}
