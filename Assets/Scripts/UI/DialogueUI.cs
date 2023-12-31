using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueSystem;
using Player;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    
    [Serializable]
    public class CharacterRes
    {
        public string characterName;
        public Sprite characterAvatar;
    }
    
    public class DialogueUI : MonoBehaviour
    {
        PlayerConversant _playerConversant;
        
        [SerializeField]
        TextMeshProUGUI aiResponseText;
        [SerializeField]
        TextMeshProUGUI speakerNameText;
        [SerializeField]
        Button nextButton;
        [SerializeField]
        Transform choiceRoot;
        [SerializeField] 
        Transform aiResponseRoot;
        [SerializeField]
        GameObject choicePrefab;
        
        [SerializeField]
        private Image playerAvatar;
        [SerializeField]
        private Image npcAvatar;
        [SerializeField]
        private Image newBackgroundImg;
        [SerializeField]
        private Image currentBackgroundImg;
        
        [SerializeField]
        private List<CharacterRes> characterAvatarRes = new();
        
        private Dictionary<string, Sprite> _dictCharacterName2AvatarRes;
        
        private PlayerStatus _playerStatus;
        

        // Find the player and get the PlayerConversant component
        // Add a listener to the next button click event
        // Add a listener to the OnConversationUpdated event
        void Awake()
        {
            GameObject.FindGameObjectWithTag("Player").TryGetComponent(out _playerConversant);
            GameObject.FindGameObjectWithTag("Player").TryGetComponent(out _playerStatus);
            nextButton.onClick.AddListener(OnClickNext);
            
            _playerConversant.OnConversationUpdated += UpdateUI;
            
            _dictCharacterName2AvatarRes = new Dictionary<string, Sprite>();
            foreach (var characterRes in characterAvatarRes)
            {
                _dictCharacterName2AvatarRes[characterRes.characterName] = characterRes.characterAvatar;
            }
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_playerConversant.IsActive() && !_playerConversant.IsChoosing())
                {
                    OnClickNext();
                }
                
            }
        }
        
        private void OnClickNext()
        {
            _playerConversant.MoveToNextNode();
        }
        
        // Method registered to the OnConversationUpdated event
        private void UpdateUI()
        {
            if (!_playerConversant.IsActive() || _playerConversant is null)
            {
                gameObject.SetActive(false);
                return;
            }
            
            var isChoosing = _playerConversant.IsChoosing();
            
            aiResponseText.text = _playerConversant.GetCurrentNodeText();
            speakerNameText.text = _playerConversant.GetCurrentSpeakerName();
            nextButton.gameObject.SetActive(!isChoosing);
            choiceRoot.gameObject.SetActive(isChoosing);
            
            if (isChoosing)
            {
                BuildChoiceList();
            }
            
            var speakerType = _playerConversant.GetCurrentSpeakerType();
            bool isPlayerSpeaker = speakerType == SpeakerType.Player;
            bool isNpcSpeaker = speakerType == SpeakerType.Npc;

            // Set the alpha value for active and inactive avatars
            float activeAlpha = 1f; // Fully visible
            float inactiveAlpha = 0.5f; // Dimmed out (you can adjust this value as needed)

            if (isPlayerSpeaker || isNpcSpeaker)
            {
                Image activeAvatar = isPlayerSpeaker ? playerAvatar : npcAvatar;
                Image inactiveAvatar = isPlayerSpeaker ? npcAvatar : playerAvatar;
                activeAvatar.sprite = _dictCharacterName2AvatarRes[_playerConversant.GetCurrentSpeakerName()];
    
                // Set the color of the active avatar with full alpha
                Color activeColor = activeAvatar.color;
                activeColor.a = activeAlpha;
                activeAvatar.color = activeColor;
    
                // Set the color of the inactive avatar with dimmed alpha
                Color inactiveColor = inactiveAvatar.color;
                inactiveColor.a = inactiveAlpha;
                inactiveAvatar.color = inactiveColor;
                
                playerAvatar.gameObject.SetActive(playerAvatar.sprite != null);
                npcAvatar.gameObject.SetActive(npcAvatar.sprite != null);
            }
            else
            {
                // Handle the case where neither avatar should be active
                playerAvatar.gameObject.SetActive(false);
                npcAvatar.gameObject.SetActive(false);
                playerAvatar.sprite = null;
                npcAvatar.sprite = null;
            }
            
            var background = _playerConversant.GetBackground();
            if (background != null)
            {
                StartCoroutine(TransitionToNewBackground(background));
            }
            
            var audioClip = _playerConversant.GetAudioClip();
            if (audioClip != null)
            {
                GameManager.Instance.PlayAudioClip(audioClip);
            }
        }
        
        private IEnumerator TransitionToNewBackground(Sprite newBackground)
        {
            newBackgroundImg.sprite = newBackground;
            float elapsedTime = 0f;
            var transitionTime = 1f; // Set this to the number of seconds you want the transition to take

            Color currentStartColor = currentBackgroundImg.color;
            Color newStartColor = new Color(newBackgroundImg.color.r, newBackgroundImg.color.g, newBackgroundImg.color.b, 0);

            while (elapsedTime < transitionTime)
            {
                elapsedTime += Time.deltaTime;
                float alpha = elapsedTime / transitionTime;

                // Interpolate the color values over time
                currentBackgroundImg.color = Color.Lerp(currentStartColor, new Color(currentStartColor.r, currentStartColor.g, currentStartColor.b, 0), alpha);
                newBackgroundImg.color = Color.Lerp(newStartColor, new Color(newStartColor.r, newStartColor.g, newStartColor.b, 1), alpha);

                yield return null; // Wait for the next frame
            }
            
            // At the end of the transition, set the current background to the next one
            currentBackgroundImg.sprite = newBackground;
            currentBackgroundImg.color = new Color(currentBackgroundImg.color.r, currentBackgroundImg.color.g, currentBackgroundImg.color.b, 1);
            newBackgroundImg.color = new Color(newBackgroundImg.color.r, newBackgroundImg.color.g, newBackgroundImg.color.b, 0);
        }


        private void BuildChoiceList()
        {
            foreach (Transform child in choiceRoot)
            {
                Destroy(child.gameObject);
            }

            foreach (var node in _playerConversant.GetChoiceNodes())
            {
                // Check node condition
                if (!_playerConversant.IsNodeStatusRequirementMet(node))
                {
                    continue;
                }
                
                var choiceButton = Instantiate(choicePrefab, choiceRoot);
                var textMeshProUGUI = choiceButton.GetComponentInChildren<TextMeshProUGUI>();
                textMeshProUGUI.text = node.GetText();
                var button = choiceButton.GetComponentInChildren<Button>();
                button.onClick.AddListener(() =>
                {
                    _playerConversant.SelectChoiceNode(node);
                });
            }
        }
    }
}
