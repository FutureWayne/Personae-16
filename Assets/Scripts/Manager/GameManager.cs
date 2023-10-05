using System;
using System.Collections.Generic;
using DialogueSystem;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private List<Dialogue> listDialogues;
    
    
    [SerializeField]
    private Transform dialogueCanvas;
    
    [SerializeField]
    private Transform StartScreenCanvas;
    
    [SerializeField]
    private Transform chapterCanvas;
    
    [SerializeField]
    private Transform summaryCanvas;

    [SerializeField] 
    private TextMeshProUGUI ChapterName;
    
    private PlayerConversant _playerConversant;
    
    private int _currentDialogueIndex = 0;
    
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("Player").TryGetComponent(out _playerConversant);
    }
    
    
    public void ResumeDialogue()
    {
        StartScreenCanvas.gameObject.SetActive(false);
        summaryCanvas.gameObject.SetActive(false);
        chapterCanvas.gameObject.SetActive(false);
        dialogueCanvas.gameObject.SetActive(true);
        _playerConversant.StartDialogue(listDialogues[_currentDialogueIndex]);
    }

    public void OnDialogueEnds()
    {
        dialogueCanvas.gameObject.SetActive(false);
        _currentDialogueIndex++;
        TransitToNextChapter();
    }
    
    public void TransitToNextChapter()
    {
        StartScreenCanvas.gameObject.SetActive(false);
        switch (_currentDialogueIndex)
        {
            case 0:
                ChapterName.text = "Prelogue: Another day of sun";
                chapterCanvas.gameObject.SetActive(true);
                // Wait for 3 seconds, then call ResumeDialogue()
                Invoke(nameof(ResumeDialogue), 3f);
                break;
            case 1:
                ChapterName.text = "Chapter 1: Someone in the crowd";
                chapterCanvas.gameObject.SetActive(true);
                // Wait for 3 seconds, then call ResumeDialogue()
                Invoke(nameof(ResumeDialogue), 3f);
                break;
            case 2:
                ChapterName.text = "Chapter 2: Take on me";
                chapterCanvas.gameObject.SetActive(true);
                // Wait for 3 seconds, then call ResumeDialogue()
                Invoke(nameof(ResumeDialogue), 3f);
                break;
            case 3:
                ChapterName.text = "Chapter 3: City of stars";
                chapterCanvas.gameObject.SetActive(true);
                // Wait for 3 seconds, then call ResumeDialogue()
                Invoke(nameof(ResumeDialogue), 3f);
                break;
            case 4:
                summaryCanvas.gameObject.SetActive(true);
                _currentDialogueIndex++;
                break;
            case 5:
                ChapterName.text = "Epilogue: You love jazz now";
                Invoke(nameof(ResumeDialogue), 3f);
                chapterCanvas.gameObject.SetActive(true);
                break;
            case 6:
                ChapterName.text = "The End";
                chapterCanvas.gameObject.SetActive(true);
                break;
            default:
                break;
        }
        
        
        
    }


    public void PlayAudioClip(AudioClip audioClip)
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.Stop();
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
