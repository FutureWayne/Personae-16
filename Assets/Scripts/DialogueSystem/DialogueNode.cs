using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueNode : ScriptableObject
    {
        [SerializeField] 
        private string text;

        private readonly List<string> _children = new List<string>();

        private Rect _rect = new Rect(0, 0, 300, 200);

        [SerializeField]
        private bool _isPlayerSpeaking;

        public List<string> GetChildren()
        {
            return _children;
        }
        
        public bool IsPlayerSpeaking()
        {
            return _isPlayerSpeaking;
        }
        
        public Rect GetRect()
        {
            return _rect;
        }

        public string GetText()
        {
            return text;
        }
        
#if UNITY_EDITOR
        public void SetText(string s)
        {
            if (!string.Equals(text, s, StringComparison.Ordinal))
            {
                Undo.RecordObject(this, "Update Dialogue Text");
                text = s;
                EditorUtility.SetDirty(this);
            }
        }

        public void SetPosition(Vector2 newNodeOffset)
        {
            Undo.RecordObject(this, "Move Dialogue Node");
            _rect.position = newNodeOffset;
            EditorUtility.SetDirty(this);
        }

        public void AddChild(string newNodeName)
        {
            Undo.RecordObject(this, "Add Dialogue Link");
            _children.Add(newNodeName);
            EditorUtility.SetDirty(this);
        }

        public void RemoveChild(string s)
        {
            Undo.RecordObject(this, "Remove Dialogue Link");
            _children.Remove(s);
            EditorUtility.SetDirty(this);
        }
        
        public void SetPlayerSpeaking(bool b)
        {
            Undo.RecordObject(this, "Change Dialogue Speaker");
            _isPlayerSpeaking = b;
            EditorUtility.SetDirty(this);
        }
#endif
        
    }

    
}
