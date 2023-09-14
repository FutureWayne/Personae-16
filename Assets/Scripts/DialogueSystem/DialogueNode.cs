using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace DialogueSystem
{
    [Serializable]
    public class DialogueNode
    {
        public string uniqueID;
        public string text;
        public string[] children;
        
        [HideInInspector]
        public Rect rect = new Rect(0,0,200,100);
    }
}
