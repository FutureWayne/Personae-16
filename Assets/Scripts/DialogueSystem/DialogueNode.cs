using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DialogueSystem
{
    [Serializable]
    public class DialogueNode
    {
        public string uniqueID;
        public string text;
        public List<string> children = new List<string>();
        
        [HideInInspector]
        public Rect rect = new Rect(0,0,200,100);
    }
}
