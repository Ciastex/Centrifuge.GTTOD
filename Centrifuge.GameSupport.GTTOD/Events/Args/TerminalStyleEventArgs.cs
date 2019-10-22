using System;
using UnityEngine;

namespace Centrifuge.GTTOD.Events.Args
{
    public class TerminalStyleEventArgs : EventArgs
    {
        public GUIStyle Style { get; private set; }

        public TerminalStyleEventArgs(GUIStyle style)
        {
            Style = style;
        }
    }
}
