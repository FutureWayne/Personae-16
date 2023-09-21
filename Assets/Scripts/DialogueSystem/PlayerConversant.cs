using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DialogueSystem
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField]
        private Dialogue newDialogue;
        
        private Dialogue _currentDialogue;
        
        private DialogueNode _currentNode;
        
        private bool _isChoosing;
        
        public event Action OnConversationUpdated; 
        
        IEnumerator Start()
        {
            yield return new WaitForSeconds(1);
            StartDialogue(newDialogue);
        }
        
        public bool IsActive()
        {
            return _currentDialogue != null;
        }

        /**********************************************
         * Call this method to start a new conversation
         **********************************************/
        private void StartDialogue(Dialogue dialogue)
        {
            _currentDialogue = dialogue;
            _currentNode = _currentDialogue.GetRootNode();
            
            OnConversationUpdated?.Invoke();
        }
        
        public string GetCurrentNodeText()
        {
            return _currentNode is null ? "" : _currentNode.GetText();
        }

        public void MoveToNextNode()
        {
            if (_currentNode is null || !HasNextNode())
            {
                return;
            }
            
            if(_currentDialogue.GetPlayerChildrenNodes(_currentNode).Any())
            {
                _isChoosing = true;
                OnConversationUpdated?.Invoke();
                return;
            }
            
            // set current node to a random child
            var children = _currentDialogue.GetAIChildrenNodes(_currentNode).ToArray();
            _currentNode = children.Length > 0 ? children[UnityEngine.Random.Range(0, children.Length)] : null;
            
            OnConversationUpdated?.Invoke();
        }
        
        public bool HasNextNode()
        {
            return _currentDialogue != null && _currentDialogue.GetAllChildrenNodes(_currentNode).Any();
        }
        
        public IEnumerable<DialogueNode> GetChoiceNodes()
        {
            return _currentDialogue.GetPlayerChildrenNodes(_currentNode);
        }
        
        public bool IsChoosing()
        {
            return _isChoosing;
        }
        
        public void SelectChoiceNode(DialogueNode chosenNode)
        {
            _currentNode = chosenNode;
            _isChoosing = false;
            MoveToNextNode();
        }
    }
}
