using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum SpeakerType
{
    Player,
    Npc,
    Narrative
}

namespace DialogueSystem
{
    [Serializable]
    public class StatusRequirement
    {
        public ECharacterStatusType statusType;
        public int requirementValue;
    }

    public class DialogueNode : ScriptableObject
    {
        [Header("Dialogue")]
        [SerializeField]
        private bool isPlayerSpeaking;
        [SerializeField]
        string text;
        [SerializeField]
        string speakerName;
        [SerializeField]
        SpeakerType speakerType = SpeakerType.Narrative;
        [SerializeField]
        Sprite background;
        
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
        private List<StatusRequirement> statusRequirementsList = new();
        
        private Dictionary<ECharacterStatusType, int> _dictStatusReq;
        
        private void OnEnable()
        {
            _dictStatusReq = new Dictionary<ECharacterStatusType, int>();
            foreach (var statusReq in statusRequirementsList)
            {
                _dictStatusReq[statusReq.statusType] = statusReq.requirementValue;
            }
        }

        public Rect GetRect()
        {
            return rect;
        }

        public string GetText()
        {
            return text;
        }
        
        public string GetSpeakerName()
        {
            return speakerName;
        }
        
        public SpeakerType GetSpeakerType()
        {
            return speakerType;
        }
        
        public Sprite GetBackground()
        {
            return background;
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
        
        public Dictionary<ECharacterStatusType, int> GetStatusReqDict()
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