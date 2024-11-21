using System.Collections.Generic;
using UnityEngine;

namespace Padoru.Core.DebugConsole
{
    public class CommandsConsoleConfig
    {
        public KeyCode ToggleConsoleKey = KeyCode.BackQuote;
        public List<KeyCode> HandleInputKeys = new () { KeyCode.KeypadEnter, KeyCode.Return };
        public Dictionary<string, BaseConsoleCommand> Commands = new();
    }
}