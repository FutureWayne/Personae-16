using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DialogueSystem
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue System/Dialogue")]
    public class Dialogue : ScriptableObject
    {
        [SerializeField]
        List<DialogueNode> nodes = new();

        private readonly Dictionary<string, DialogueNode> _nodeLookup = new();

        #if UNITY_EDITOR
        private void Awake()
        {
            if (nodes.Count == 0)
            {
                nodes.Add(new DialogueNode());
            }
            
            OnValidate();
        }
        #endif

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
    }
}
