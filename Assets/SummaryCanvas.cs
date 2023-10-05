using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using TMPro;
using UnityEngine;

public class SummaryCanvas : MonoBehaviour
{
    private PlayerStatus _playerStatus;

    [SerializeField]
    private TextMeshProUGUI _text;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        _playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
        var numAlign = _playerStatus.numGoAlongPersona;
        var numContrary = _playerStatus.numGoAgainstPersona;
        _text.text = $"Aligned Choices: {numAlign} Contrary Choices: {numContrary}";
    }
}
