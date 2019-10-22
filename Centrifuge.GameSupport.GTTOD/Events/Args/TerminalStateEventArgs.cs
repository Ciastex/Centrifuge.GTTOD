using System;

namespace Centrifuge.GTTOD.Events.Args
{
    public class TerminalStateEventArgs : EventArgs
    {
        public CommandTerminal.Terminal Terminal { get; }

        public TerminalStateEventArgs(CommandTerminal.Terminal terminal)
        {
            Terminal = terminal;
        }
    }
}
