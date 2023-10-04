using System.Collections.Generic;
using Player;
using DialogueSystem;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Buff
{
    public class BuffReceiver : MonoBehaviour
    {
        private Dictionary<BuffData, List<int>> _dictBuffDuration = new();
        
        private PlayerStatus _playerStatus;
        private string _playerPersonalityType;
        private PlayerConversant _playerConversant;
        public string result = "";
        public TMP_Text personalityResults;
        
        private void Start()
        {            
            _playerStatus = GetComponent<PlayerStatus>();
            _playerPersonalityType = _playerStatus.PersonalityType;
            
            _playerConversant = GetComponent<PlayerConversant>();
            _playerConversant.OnConversationUpdated += BuffEffect;
        }
        
        public void AddBuff(BuffData buffData)
        {
            //Debug.Log(buffData.id);
            if (buffData.id > 1000)
            {
                // Modify the personality type
                
                result += buffData.buffName;
                _playerStatus.PersonalityType = result;
                //Debug.Log(_playerStatus.PersonalityType);
                personalityResults.SetText("It looks like your personality type is " + result);
                return;
            }


            if (_dictBuffDuration.ContainsKey(buffData))
            {
                _dictBuffDuration[buffData].Add(buffData.duration);
            }
            else
            {
                _dictBuffDuration.Add(buffData, new List<int>() {buffData.duration});
            }
        }

        public void RemoveBuff(BuffData buffData)
        {
            if (_dictBuffDuration.ContainsKey(buffData))
            {
                _dictBuffDuration.Remove(buffData);
            }
        }

        private void BuffEffect()
        {
            var debugstr = "";
            
            var listBuffDataToRemove = new List<BuffData>();
            foreach (var buffKeyValuePair in _dictBuffDuration)
            {
                var buffData = buffKeyValuePair.Key;
                var durations = buffKeyValuePair.Value;

                for (int i = durations.Count - 1; i >= 0; i--)
                {
                    if (durations[i] <= 0)
                    {
                        durations.RemoveAt(i);
                        continue;
                    }

                    durations[i]--;

                    var aspect = buffData.aspect;
                    var modifierA = buffData.modifierA;
                    var modifierB = buffData.modifierB;

                    var playerSideOfAspect = PersonalityHelper.GetAspectSideByType(aspect, _playerPersonalityType);
                    var inEffectModifier = playerSideOfAspect == 0 ? modifierA : modifierB;
                    var affectedStatus = PersonalityHelper.GetStatusByAspect(aspect);
                    _playerStatus.SetStatus(affectedStatus, _playerStatus.GetStatus(affectedStatus) + inEffectModifier);

                    var durationTime = durations[i];
                    debugstr +=
                        $"{buffData.buffName} - {aspect} - {affectedStatus} - {inEffectModifier} - {durationTime}\n";
                }
                
                Debug.LogWarning(debugstr);
            }

            foreach (var buffData in listBuffDataToRemove)
            {
                _dictBuffDuration.Remove(buffData);
            }
        }
    }
}
