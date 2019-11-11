using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Centrifuge.GTTOD.ResourceManagement
{
    internal class PrefabInitializer
    {
        private Dictionary<string, Scene> Scenes { get; }
        private int PreviousCount { get; set; }

        internal PrefabInitializer()
        {
            Scenes = new Dictionary<string, Scene>();
        }

        internal void HookUpEvent()
        {
            SceneManager.sceneLoaded += RegisterGameSpecificPrefabs;
        }

        private void RegisterGameSpecificPrefabs(Scene scene, LoadSceneMode mode)
        {
            if (Scenes.ContainsKey(scene.name))
                return;

            Scenes.Add(scene.name, scene);

            var rgx = new Regex(@"(\(\d+\))$");
            var objs = Resources.FindObjectsOfTypeAll<GameObject>()
                                .Where(
                                    x => !rgx.IsMatch(x.name) &&
                                    x.scene.name == null // prefabs don't get a reference to the scene
                                );                       // so that's how you identify them

            foreach (var o in objs)
            {
                if (GttodAssets.Instance.HasAsset(o.name))
                    continue;

                GameAPI.Log.Info($"Registering prefab: {o.name}");
                GttodAssets.Instance.AddAsset(o);
            }

            if (PreviousCount != GttodAssets.Instance.TotalAssets())
            {
                GttodAssets.Instance.OnAssetsInitialized(scene);
                GameAPI.Log.Info($"Discovered and registered {GttodAssets.Instance.TotalAssets()} new prefabs from scene '{scene.name}'.");
            }
            PreviousCount = GttodAssets.Instance.TotalAssets();
        }
    }
}
