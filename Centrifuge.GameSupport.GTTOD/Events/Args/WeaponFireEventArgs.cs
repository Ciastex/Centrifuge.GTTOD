using UnityEngine;

namespace Centrifuge.GTTOD.Events.Args
{
    public class WeaponFireEventArgs : TypeInstanceEventArgs<GameObject>
    {
        public bool IsPrimaryFire { get; internal set; }
        public int BurstCount { get; internal set; }

        public WeaponFireEventArgs(GameObject instance) : base(instance) { }
    }
}
