using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue System/Dialogue")]
    public class Dialogue : ScriptableObject
    {
        [SerializeField]
        List<DialogueNode> nodes = new();

        #if UNITY_EDITOR
        private void Awake()
        {
            if (nodes.Count == 0)
            {
                nodes.Add(new DialogueNode());
            }
        }
        #endif
        
        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
        }
        
        public DialogueNode GetRootNode()
        {
            return nodes[0];
        }
    }
}
