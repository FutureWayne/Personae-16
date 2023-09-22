using System;
using UnityEditor;
using UnityEngine;

namespace Buff
{
    public static class BuffCfgReader
    {
        [MenuItem("Tools/Import Buffs from CSV")]
        public static void ImportBuffs()
        {
            TextAsset csvFile = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Buff.csv");

            string[] lines = csvFile.text.Split('\n');
            for (int i = 1; i < lines.Length; i++)
            {
                if(string.IsNullOrWhiteSpace(lines[i])) continue;  // Skip empty lines
                
                string[] fields = lines[i].Split(',');
                try
                {
                    BuffData buff = ScriptableObject.CreateInstance<BuffData>();
                    {
                        buff.id = int.Parse(fields[0]);
                        buff.buffName = fields[1];
                    
                        buff.aspect = (EPersonalityAspects) (int.Parse(fields[2]) - 1);
                        buff.modifierA = int.Parse(fields[3]);
                        buff.modifierB = int.Parse(fields[4]);
                    
                        string assetPath = $"Assets/Game/Buffs/{buff.buffName}.asset";
                        AssetDatabase.CreateAsset(buff, assetPath);
                    };
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error parsing line {i + 1}, column {Array.FindIndex(fields, f => f == fields[fields.Length - 1]) + 1}. Error: {e.Message}");
                    throw;
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}