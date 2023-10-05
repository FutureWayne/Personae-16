using System;
using System.Collections.Generic;
using DialogueSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private List<Dialogue> listDialogues;
    
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
        
        
        _playerConversant.StartDialogue(listDialogues[_currentDialogueIndex]);
       
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDialogueEnds()
    {
        _currentDialogueIndex++;
        _playerConversant.StartDialogue(listDialogues[_currentDialogueIndex]);
    }
}
