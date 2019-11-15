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
        public static GttodAssets Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GttodAssets();

                return _instance;
            }
        }

        private Dictionary<string, GameObject> GameObjects { get; }

        public int TotalAssets => GameObjects.Count;

        public List<string> PrefabNames => GameObjects.Keys.ToList();
        public List<GameObject> Prefabs => GameObjects.Values.ToList();

        public static event EventHandler<PrefabsInitializationEventArgs> AssetsInitialized;

        public bool HasAsset(string name)
            => GameObjects.ContainsKey(name);

        public GameObject Retrieve(string name)
        {
            if (!HasAsset(name))
                return null;

            return GameObjects[name];
        } 

        internal GttodAssets()
            => GameObjects = new Dictionary<string, GameObject>();

        internal void OnAssetsInitialized(Scene scene)
            => AssetsInitialized?.Invoke(this, new PrefabsInitializationEventArgs(scene));

        internal void AddAsset(GameObject prefab)
        {
            if (!HasAsset(prefab.name))
                GameObjects.Add(prefab.name, prefab);
        }
    }
}
