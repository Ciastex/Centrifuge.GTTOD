using Centrifuge.GTTOD.Events.Args;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Centrifuge.GTTOD.ResourceManagement
{
    public class GttodAssets
    {
        private static GttodAssets _instance;
        private Dictionary<string, GameObject> _gameObjects;

        public static GttodAssets Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GttodAssets();

                return _instance;
            }
        }

        public static event EventHandler<PrefabsInitializationEventArgs> AssetsInitialized;

        internal GttodAssets()
            => _gameObjects = new Dictionary<string, GameObject>();


        internal void OnAssetsInitialized(Scene scene)
            => AssetsInitialized?.Invoke(this, new PrefabsInitializationEventArgs(scene));

        internal void AddAsset(GameObject prefab)
        {
            if (!HasAsset(prefab.name))
                _gameObjects.Add(prefab.name, prefab);
        }

        public bool HasAsset(string name)
            => _gameObjects.ContainsKey(name);

        public int TotalAssets()
            => _gameObjects.Count;

        public List<string> GetAvailablePrefabNames()
            => _gameObjects.Keys.ToList();

        public GameObject Retrieve(string name)
        {
            if (!HasAsset(name))
                return null;

            return _gameObjects[name];
        }
    }
}
