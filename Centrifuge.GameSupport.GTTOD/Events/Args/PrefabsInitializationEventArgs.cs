using System;
using UnityEngine.SceneManagement;

namespace Centrifuge.GTTOD.Events.Args
{
    public class PrefabsInitializationEventArgs : EventArgs
    {
        public Scene Scene { get; }

        public PrefabsInitializationEventArgs(Scene scene)
        {
            Scene = scene;
        }
    }
}
