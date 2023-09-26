using System.Collections.Generic;
using UnityEngine;

public enum ECharacterStatus
{
    Energy,
    Stress,
    Motivation,
    Confidence,
}

public enum EPersonalityAspects
{
    Mind,
    Energy,
    Nature,
    Tactics
}

namespace Player
{
    public class PlayerStatus : MonoBehaviour
    {
        private readonly Dictionary<ECharacterStatus, int> _dictStatus = new();
        
        [SerializeField]
        private string _personalityType;  // 4 letters, e.g. "INTP"
        
        private void Awake()
        {
            InitStatus();
        }
        
        public void SetStatus(ECharacterStatus status, int value)
        {
            _dictStatus[status] = value;
        }

        public int GetStatus(ECharacterStatus status)
        {
            return _dictStatus[status];
        }
        
        // TODO: Init status from cfg
        public void InitStatus()
        {
            _dictStatus[ECharacterStatus.Energy] = 100;
            _dictStatus[ECharacterStatus.Stress] = 0;
            _dictStatus[ECharacterStatus.Motivation] = 100;
            _dictStatus[ECharacterStatus.Confidence] = 100;
        }
        
        public string PersonalityType
        {
            get => _personalityType;
            set => _personalityType = value;
        }
    }
}