using System.Collections.Generic;
using Player;
using DialogueSystem;
using UnityEngine;

namespace Buff
{
    public class BuffReceiver : MonoBehaviour
    {
        private readonly List<BuffData> _listBuffReceived = new();
        
        private PlayerStatus _playerStatus;
        private string _playerPersonalityType;
        private PlayerConversant _playerConversant;
        
        private void Start()
        {            
            _playerStatus = GetComponent<PlayerStatus>();
            _playerPersonalityType = _playerStatus.PersonalityType;
            
            _playerConversant = GetComponent<PlayerConversant>();
            _playerConversant.OnConversationUpdated += BuffEffect;
        }
        
        public void AddBuff(BuffData buffData)
        {
            _listBuffReceived.Add(buffData);
        }

        public void RemoveBuff(BuffData buffData)
        {
            if (_listBuffReceived.Contains(buffData))
            {
                _listBuffReceived.Remove(buffData);
            }
        }

        private void BuffEffect()
        {
            var debugstr = "";
            foreach (var buffData in _listBuffReceived)
            {
                var aspect = buffData.aspect;
                var modifierA = buffData.modifierA;
                var modifierB = buffData.modifierB;
                
                var playerSideOfAspect = PersonalityHelper.GetAspectSideByType(aspect, _playerPersonalityType);
                var inEffectModifier = playerSideOfAspect == 0 ? modifierA : modifierB;
                var affectedStatus = PersonalityHelper.GetStatusByAspect(aspect);
                _playerStatus.SetStatus(affectedStatus, _playerStatus.GetStatus(affectedStatus) + inEffectModifier);
                
                debugstr += $"{buffData.buffName} - {aspect} - {affectedStatus} - {inEffectModifier}\n";
            }
            
            Debug.LogWarning(debugstr);
        }
    }
}
