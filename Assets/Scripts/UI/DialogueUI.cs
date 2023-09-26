using UnityEngine;
using DialogueSystem;
using TMPro;
using UnityEngine.UI;

namespace UI
{
    public class DialogueUI : MonoBehaviour
    {
        PlayerConversant _playerConversant;
        
        [SerializeField]
        TextMeshProUGUI aiResponseText;
        [SerializeField]
        Button nextButton;
        [SerializeField]
        Transform choiceRoot;
        [SerializeField] 
        Transform aiResponseRoot;
        [SerializeField]
        GameObject choicePrefab;

        // Find the player and get the PlayerConversant component
        // Add a listener to the next button click event
        // Add a listener to the OnConversationUpdated event
        void Start()
        {
            GameObject.FindGameObjectWithTag("Player").TryGetComponent(out _playerConversant);
            nextButton.onClick.AddListener(OnClickNext);
            
            _playerConversant.OnConversationUpdated += UpdateUI;
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
            aiResponseRoot.gameObject.SetActive(!isChoosing);
            choiceRoot.gameObject.SetActive(isChoosing);
            
            if (isChoosing)
            {
                BuildChoiceList();
            }
            else
            {
                aiResponseText.text = _playerConversant.GetCurrentNodeText();
                nextButton.gameObject.SetActive(_playerConversant.HasNextNode());
            }
        }

        private void BuildChoiceList()
        {
            foreach (Transform child in choiceRoot)
            {
                Destroy(child.gameObject);
            }

            foreach (var node in _playerConversant.GetChoiceNodes())
            {
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