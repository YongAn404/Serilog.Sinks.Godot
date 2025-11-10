using Godot;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;
using Serilog.Sinks.Godot.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serilog.Sinks.Godot
{
    public static class GodotSinkExtensions
    {
        private const string DefaultDebugOutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}";

        public static LoggerConfiguration Godot(this LoggerSinkConfiguration sinkConfiguration, LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum, string outputTemplate = DefaultDebugOutputTemplate, IFormatProvider? formatProvider = null, LoggingLevelSwitch? levelSwitch = null)
        {
            ArgumentNullException.ThrowIfNull(sinkConfiguration);
            ArgumentNullException.ThrowIfNull(outputTemplate);

            var formatter = new MessageTemplateTextFormatter(outputTemplate, formatProvider);
            return sinkConfiguration.Godot(formatter, restrictedToMinimumLevel, levelSwitch);
        }
        public static LoggerConfiguration Godot(this LoggerSinkConfiguration sinkConfiguration, IGameConsole? gameConsole, LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum, string outputTemplate = DefaultDebugOutputTemplate, IFormatProvider? formatProvider = null, LoggingLevelSwitch? levelSwitch = null)
        {
            ArgumentNullException.ThrowIfNull(sinkConfiguration);
            ArgumentNullException.ThrowIfNull(outputTemplate);

            var formatter = new MessageTemplateTextFormatter(outputTemplate, formatProvider);
            return sinkConfiguration.Godot(gameConsole, formatter, restrictedToMinimumLevel, levelSwitch);
        }
        public static LoggerConfiguration Godot(this LoggerSinkConfiguration sinkConfiguration, ITextFormatter formatter, LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum, LoggingLevelSwitch? levelSwitch = null)
        {
            ArgumentNullException.ThrowIfNull(sinkConfiguration);
            ArgumentNullException.ThrowIfNull(formatter);
            return sinkConfiguration.Godot(null, formatter, restrictedToMinimumLevel, levelSwitch);
        }

        public static LoggerConfiguration Godot(this LoggerSinkConfiguration sinkConfiguration, IGameConsole? gameConsole , ITextFormatter formatter, LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum, LoggingLevelSwitch? levelSwitch = null)
        {
            ArgumentNullException.ThrowIfNull(sinkConfiguration);
            ArgumentNullException.ThrowIfNull(formatter);
            return sinkConfiguration.Sink(new GodotSink(formatter, gameConsole), restrictedToMinimumLevel, levelSwitch);
        }
    }
}
