using Centrifuge.GTTOD.Events;
using Reactor.API.Attributes;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GslTestMod
{
    [ModEntryPoint("com.github.ciastex/GTTOD.GslTestMod")]
    public class Mod : MonoBehaviour
    {
        private Log Log = LogManager.GetForCurrentAssembly();

        public void Initialize(IManager manager)
        {
            Weapon.PreviewAwake += (sender, e) =>
            {
                SeemsToWork("Weapon.PreviewAwake");
            };

            Weapon.AwakeComplete += (sender, e) =>
            {
                SeemsToWork("Weapon.AwakeComplete");
            };

            Weapon.PreviewPrimaryShot += (sender, e) =>
            {
                SeemsToWork("Weapon.PreviewPrimaryShot");
            };

            Weapon.PreviewSecondaryShot += (sender, e) =>
            {
                SeemsToWork("Weapon.PreviewSecondaryShot");
            };

            Weapon.ShotFired += (sender, e) =>
            {
                SeemsToWork("Weapon.ShotFired");
            };

            Player.Died += (sender, e) =>
            {
                SeemsToWork("Player.Died");
            };

            Game.GameModeStarted += (sender, e) =>
            {
                SeemsToWork("Game.GameModeStarted");
            };

            Game.PauseMenuOpened += (sender, e) =>
            {
                SeemsToWork("Game.PauseMenuOpened");
            };

            Game.PauseMenuClosed += (sender, e) =>
            {
                SeemsToWork("Game.PauseMenuClosed");
            };

            EnemyNPC.DroneDied += (sender, e) =>
            {
                SeemsToWork("EnemyNPC.DroneDied");
            };

            EnemyNPC.InfantryDied += (sender, e) =>
            {
                SeemsToWork("EnemyNPC.InfantryDied");
            };

            EnemyNPC.TurretCrabDied += (sender, e) =>
            {
                SeemsToWork("EnemyNPC.TurretCrabDied");
            };
        }

        private void SeemsToWork(string name)
        {
            Log.Info($"{name} seems to work.");
        }
    }
}
