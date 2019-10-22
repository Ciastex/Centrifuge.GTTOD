using System;

namespace Centrifuge.GTTOD.Events
{
    public static class Game
    {
        public static event EventHandler GameModeStarted;
        public static event EventHandler PauseMenuOpened;
        public static event EventHandler PauseMenuClosed;

        internal static void InvokeGameModeStarted()
            => GameModeStarted?.Invoke(null, EventArgs.Empty);

        internal static void InvokePauseMenuOpened()
            => PauseMenuOpened?.Invoke(null, EventArgs.Empty);

        internal static void InvokePauseMenuClosed()
            => PauseMenuClosed?.Invoke(null, EventArgs.Empty);
    }
}
