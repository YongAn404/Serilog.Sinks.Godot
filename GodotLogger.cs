using Godot;
using Godot.Collections;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serilog.Sinks.Godot
{
    public partial class GodotLogger : Logger
    {
        private const string Prefix = "Serilog|";
        
        public override void _LogError(string function, string file, int line, string code, string rationale, bool editorNotify, int errorType, Array<ScriptBacktrace> scriptBacktraces)
        {
            Log.Error($"{{{(ErrorType)errorType}}} '{file}' => {function} {line}\n" +
                $"{(string.IsNullOrEmpty(rationale) ? $"rationale: {rationale}\n" : "")}" +
                $"{code}\n" +
                $"------Backtrace------" +
                $"{string.Join("\n", scriptBacktraces.Select((sb) => sb.Format()))}" +
                $"\n---------End---------\n" +
                $"editorNotify: {editorNotify}\n");
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
