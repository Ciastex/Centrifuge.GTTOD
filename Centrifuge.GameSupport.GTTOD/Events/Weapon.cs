using Centrifuge.GTTOD.Events.Args;
using System;

namespace Centrifuge.GTTOD.Events
{
    public class Weapon
    {
        public static event EventHandler<MethodPreviewEventArgs<WeaponScript>> PreviewAwake;
        public static event EventHandler<TypeInstanceEventArgs<WeaponScript>> AwakeComplete;

        public static event EventHandler<MethodPreviewEventArgs<WeaponScript>> PreviewPrimaryShot;
        public static event EventHandler<MethodPreviewEventArgs<WeaponScript>> PreviewSecondaryShot;

        public static event EventHandler<WeaponFireEventArgs> ShotFired;

        internal static void InvokePreviewAwake(MethodPreviewEventArgs<WeaponScript> e)
            => PreviewAwake?.Invoke(null, e);

        internal static void InvokeAwakeComplete(TypeInstanceEventArgs<WeaponScript> e)
            => AwakeComplete?.Invoke(null, e);

        internal static void InvokePreviewShotFiredPrimary(MethodPreviewEventArgs<WeaponScript> e)
            => PreviewPrimaryShot?.Invoke(null, e);

        internal static void InvokePreviewShotFiredSecondary(MethodPreviewEventArgs<WeaponScript> e)
            => PreviewSecondaryShot?.Invoke(null, e);

        internal static void InvokeShotFiredPrimary(WeaponScript weapon, int burstCount)
            => ShotFired?.Invoke(null, new WeaponFireEventArgs(weapon.gameObject) { IsPrimaryFire = true, BurstCount = burstCount });

        internal static void InvokeShotFiredSecondary(WeaponScript weapon, int burstCount)
            => ShotFired?.Invoke(null, new WeaponFireEventArgs(weapon.gameObject) { IsPrimaryFire = false, BurstCount = burstCount });
    }
}

