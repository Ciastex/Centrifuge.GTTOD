using System;
using System.Collections.Generic;

namespace Centrifuge.GTTOD.Events.Args
{
    public class TypeInstanceEventArgs<T> : EventArgs
    {
        public T Instance { get; }
        public Dictionary<string, object> AdditionalData { get; }

        public TypeInstanceEventArgs(T instance)
        {
            Instance = instance;
            AdditionalData = new Dictionary<string, object>();
        }
    }
}
