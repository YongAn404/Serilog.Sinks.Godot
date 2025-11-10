using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serilog.Sinks.Godot.Interfaces
{
    public interface IGameConsole
    {
        public void AddLog(string text);
    }
}
