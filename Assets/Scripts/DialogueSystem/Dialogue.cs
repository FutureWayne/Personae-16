using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static System.Guid;

namespace DialogueSystem
{
    public class Dialogue : ScriptableObject
    {
        [SerializeField] 
        private List<DialogueNode> nodes = new List<DialogueNode>();

        private readonly Dictionary<string, DialogueNode> _nodeLookup = new();
        
        private void OnValidate()
        {
            _nodeLookup.Clear();
            foreach (var node in GetAllNodes())
            {
                _nodeLookup[node.uniqueID] = node;
            }
        }

        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
        }
        
        public DialogueNode GetRootNode()
        {
            return nodes[0];
        }

        public IEnumerable GetAllChildren(DialogueNode parentNode)
        {
            return parentNode.children.Select(childUniqueID => _nodeLookup[childUniqueID]);
        }

        public void InitializeRootNode()
        {
            if (nodes.Count == 0)
            {
                DialogueNode node = new DialogueNode
                {
                    text = "Start",
                    uniqueID = NewGuid().ToString()
                };
                nodes.Add(node);
            }
        }

        public void CreateNode(DialogueNode parentNode)
        {
            DialogueNode newNode = new DialogueNode
            {
                text = "New Node",
                uniqueID = NewGuid().ToString(),
                rect = new Rect(parentNode.rect.position + new Vector2(300, 0), new Vector2(200, 100))
            };
            nodes.Add(newNode);
            parentNode.children.Add(newNode.uniqueID);
            OnValidate();
        }
        
        public void DeleteNode(DialogueNode nodeToDelete)
        {
            nodes.Remove(nodeToDelete);
            OnValidate();
            CleanDanglingChildren(nodeToDelete);
        }

        private void CleanDanglingChildren(DialogueNode nodeToDelete)
        {
            foreach (var node in GetAllNodes())
            {
                node.children.Remove(nodeToDelete.uniqueID);
            }
        }
    }
    
    public static class DialogueCreator
    {
        [MenuItem("Assets/Create/DialogueSystem/Dialogue")]
        public static void CreateDialogue()
        {
            Dialogue dialogueAsset = ScriptableObject.CreateInstance<Dialogue>();
            
            dialogueAsset.InitializeRootNode();

            // Create a new asset file at the selected path and save the asset to it.
            ProjectWindowUtil.CreateAsset(dialogueAsset, "New Dialogue.asset");
            AssetDatabase.SaveAssets();
            
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = dialogueAsset;
        }
    }
}
