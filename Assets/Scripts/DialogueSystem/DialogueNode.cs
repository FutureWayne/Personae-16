using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueNode : ScriptableObject
    {
        [SerializeField]
        private bool isPlayerSpeaking;
        [SerializeField]
        string text;
        
        [Header("Buff Effect")]
        [SerializeField]
        List<int> listAddBuffID;
        [SerializeField]
        List<int> listRemoveBuffID;
                
        [SerializeField][HideInInspector]
        Rect rect = new Rect(0, 0, 200, 100);
        
        [SerializeField][HideInInspector]
        List<string> children = new();
        
        [Header("Status Requirement")]
        [SerializeField] 
        private int status1Req;
        [SerializeField]
        private int status2Req;
        [SerializeField]
        private int status3Req;
        [SerializeField]
        private int status4Req;
        
        private Dictionary<ECharacterStatus, int> _dictStatusReq;


        private void OnEnable()
        {
            _dictStatusReq = new Dictionary<ECharacterStatus, int>
            {
                [ECharacterStatus.Energy] = status1Req,
                [ECharacterStatus.Stress] = status2Req,
                [ECharacterStatus.Motivation] = status3Req,
                [ECharacterStatus.Confidence] = status4Req
            };
        }


        public Rect GetRect()
        {
            return rect;
        }

        public string GetText()
        {
            return text;
        }

        public List<string> GetChildren()
        {
            return children;
        }

        public bool IsPlayerSpeaking()
        {
            return isPlayerSpeaking;
        }
        
        public IEnumerable<int> GetAddBuffIDList()
        {
            return listAddBuffID;
        }
        
        public IEnumerable<int> GetRemoveBuffIDList()
        {
            return listRemoveBuffID;
        }
        
        public Dictionary<ECharacterStatus, int> GetStatusReqDict()
        {
            return _dictStatusReq;
        }

#if UNITY_EDITOR
        public void SetPosition(Vector2 newPosition)
        {
            Undo.RecordObject(this, "Move Dialogue Node");
            rect.position = newPosition;
            EditorUtility.SetDirty(this);
        }

        public void SetText(string newText)
        {
            if (newText != text)
            {
                Undo.RecordObject(this, "Update Dialogue Text");
                text = newText;
                EditorUtility.SetDirty(this);
            }
        }

        public void AddChild(string childID)
        {
            Undo.RecordObject(this, "Add Dialogue Link");
            children.Add(childID);
            EditorUtility.SetDirty(this);
        }

        public void RemoveChild(string childID)
        {
            Undo.RecordObject(this, "Remove Dialogue Link");
            children.Remove(childID);
            EditorUtility.SetDirty(this);
        }

        public void SetPlayerSpeaking(bool newIsPlayerSpeaking)
        {
            Undo.RecordObject(this, "Change Dialogue Speaker");
            isPlayerSpeaking = newIsPlayerSpeaking;
            EditorUtility.SetDirty(this);
        }
#endif
    }
}