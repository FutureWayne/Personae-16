using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using TMPro;

public class ScriptReader : MonoBehaviour
{
    [SerializeField]
    private TextAsset _InkJsonFile;
    private Story _StoryScript;

    public TMP_Text dialogueBox;
    public TMP_Text nameTag;

    [SerializeField]
    private GridLayoutGroup choiceHolder;

    [SerializeField]
    private Button choiceBasePrefab;
    // Start is called before the first frame update
    void Start()
    {
        LoadStory();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DisplayNextLine();
        }
    }

    void LoadStory()
    {
        _StoryScript = new Story(_InkJsonFile.text);

        _StoryScript.BindExternalFunction("Name", (string charName) => ChangeName(charName));
    }

    public void DisplayNextLine()
    {
        if(_StoryScript.canContinue) // Checking if there is content to go through
        {
            string text = _StoryScript.Continue(); // Gets Next Line
            text = text?.Trim(); // Removes White Space from the text
            dialogueBox.text = text; // Displays new text
        }
        else if(_StoryScript.currentChoices.Count > 0)
        {
            DisplayChoices();
        }
        else
        {
            dialogueBox.text = "That's all folks"; // Display when there is no text.
        }
    }

    private void DisplayChoices()
    {
        if (choiceHolder.GetComponentsInChildren<Button>().Length > 0) return; // Checks if Button holder has buttons in it.

        foreach(var choice in _StoryScript.currentChoices)
        {
            var button = CreateChoiceButton(choice.text); // creates a choice button

            button.onClick.AddListener(() => OnClickChoiceButton(choice));
        }
    }


    Button CreateChoiceButton(string text)
    {
        // Instantiate the button Prefab
        var choiceButton = Instantiate(choiceBasePrefab);
        choiceButton.transform.SetParent(choiceHolder.transform, false);

        // Change the text in the button Prefab.
        var buttonText = choiceButton.GetComponentInChildren<TMP_Text>();
        buttonText.text = text;

        return choiceButton;
    }


    void OnClickChoiceButton(Choice choice)
    {
        _StoryScript.ChooseChoiceIndex(choice.index);
        DestroyChoiceView();
        DisplayNextLine();
    }


    void DestroyChoiceView()
    {
        if(choiceHolder != null)
        {
            foreach(var button in choiceHolder.GetComponentsInChildren<Button>())
            {
                Destroy(button.gameObject);
            }
        }
    }


    public void ChangeName(string speakerName)
    {
        nameTag.text = speakerName;
    }
}
