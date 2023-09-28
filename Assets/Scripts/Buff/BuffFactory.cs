#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Collections.Generic;
using UnityEngine;

namespace Buff
{
    public static class BuffFactory
    {
        private static readonly Dictionary<int, BuffData> DictBuffData = new();

        static BuffFactory()
        {
            LoadAllBuffData();
        }

        private static void LoadAllBuffData()
        {
#if UNITY_EDITOR
            // Load all BuffData ScriptableObjects from the designated path
            string[] guids = AssetDatabase.FindAssets("t:BuffData", new[] {"Assets/Game/Buffs"});
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                BuffData buffData = AssetDatabase.LoadAssetAtPath<BuffData>(assetPath);
                if (buffData != null)
                {
                    DictBuffData[buffData.id] = buffData;
                }
            }
#else
        Debug.LogError("Buff loading outside Resources is not supported in builds.");
#endif
        }
        
        public static IBuffEffect GetBuffByID(int buffID)
        {
            if (DictBuffData.TryGetValue(buffID, out var buffData))
            {
                return buffData;
            }
            else
            {
                Debug.LogError($"Buff ID {buffID} not found!");
                return null;
            }
        }
    }
}
