using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Buff;
using Player;
using UnityEngine;

namespace DialogueSystem
{
    public class PlayerConversant : MonoBehaviour
    {
        private Dialogue _currentDialogue;
        
        private DialogueNode _currentNode;
        
        private bool _isChoosing;
        
        private PlayerStatus _playerStatus;
        
        public event Action OnConversationUpdated; 
        
        void Start()
        {
            _playerStatus = GetComponent<PlayerStatus>();
        }

        public bool IsActive()
        {
            return _currentDialogue != null;
        }

        /**********************************************
         * Call this method to start a new conversation
         **********************************************/
        public void StartDialogue(Dialogue dialogue)
        {
            _currentDialogue = dialogue;
            _currentNode = _currentDialogue.GetRootNode();
            
            TriggerEnterAction();
            OnConversationUpdated?.Invoke();
        }
        
        public void Quit()
        {
            _currentDialogue = null;
            _currentNode = null;
            _isChoosing = false;
            
            TriggerExitAction();
            OnConversationUpdated?.Invoke();
        }
        
        public string GetCurrentNodeText()
        {
            return _currentNode is null ? "" : _currentNode.GetText();
        }
        
        public string GetCurrentSpeakerName()
        {
            return _currentNode is null ? "" : _currentNode.GetSpeakerName();
        }
        
        public SpeakerType GetCurrentSpeakerType()
        {
            return _currentNode.GetSpeakerType();
        }

        public Sprite GetBackground()
        {
            return _currentNode.GetBackground();
        }
        
        public AudioClip GetAudioClip()
        {
            return _currentNode.GetAudioClip();
        }
        
        public string GetPersonalityType()
        {
            return _currentNode.GetPersonalityType();
        }

        public void MoveToNextNode()
        {
            if (_currentNode is null || !HasNextNode())
            {
                GameManager.Instance.OnDialogueEnds();
                return;
            }
            
            if(_currentDialogue.GetPlayerChildrenNodes(_currentNode).Any())
            {
                _isChoosing = true;
                TriggerExitAction();
                OnConversationUpdated?.Invoke();
                return;
            }
            
            TriggerExitAction();
            
            // set current node to a random child
            var children = _currentDialogue.GetAIChildrenNodes(_currentNode).ToArray();
            _currentNode = children.Length > 0 ? children[UnityEngine.Random.Range(0, children.Length)] : null;
            
            TriggerEnterAction();
            
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
            
            string personalityType = GetPersonalityType();
            if (!string.IsNullOrEmpty(personalityType))
            {
                _playerStatus.ReceivePersonalityType(personalityType);
            }
            
            TriggerEnterAction();
            _isChoosing = false;
            MoveToNextNode();
        }

        private void TriggerEnterAction()
        {
            
        }

        private void TriggerExitAction()
        {
            if (_currentNode is null)
            {
                return;
            }
            
            foreach (var addBuffID in _currentNode.GetAddBuffIDList())
            {
                var buff = BuffFactory.GetBuffByID(addBuffID);
                buff.AddBuff(gameObject);
            }
            
            foreach (var removeBuffID in _currentNode.GetRemoveBuffIDList())
            {
                var buff = BuffFactory.GetBuffByID(removeBuffID);
                buff.RemoveBuff(gameObject);
            }
        }
        
        public bool IsNodeStatusRequirementMet(DialogueNode node)
        {
            // Only check player choices
            if (!node.IsPlayerSpeaking())
            {
                return true;
            }
            
            var dictStatusReq = node.GetStatusReqDict();
            if (dictStatusReq is null)
            {
                return true;
            }

            // Use Linq to check if all status requirement is met
            return dictStatusReq.All(pair => _playerStatus.GetStatusByType(pair.Key) >= pair.Value);
        }
    }
}
