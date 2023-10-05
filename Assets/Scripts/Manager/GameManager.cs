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

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("Player").TryGetComponent(out _playerConversant);
        
        
        _playerConversant.StartDialogue(listDialogues[0]);
       
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
