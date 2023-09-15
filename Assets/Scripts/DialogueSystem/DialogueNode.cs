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
        
        public Rect rect = new Rect(0,0,400,400);
    }
}
