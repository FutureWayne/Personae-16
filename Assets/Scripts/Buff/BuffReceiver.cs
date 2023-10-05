using System.Collections.Generic;
using Player;
using DialogueSystem;
using UnityEngine;
using TMPro;

namespace Buff
{
    public class BuffReceiver : MonoBehaviour
    {
        private readonly Dictionary<BuffData, List<int>> _dictBuffDuration = new();
        
        private PlayerStatus _playerStatus;
        private PlayerConversant _playerConversant;

        public string result = "";
        public TMP_Text personalityResults;
        
        private void Start()
        {            
            _playerStatus = GetComponent<PlayerStatus>();
            
            _playerConversant = GetComponent<PlayerConversant>();
            _playerConversant.OnConversationUpdated += BuffEffect;
        }
        
        public void AddBuff(BuffData buffData)
        {
            if (buffData.id > 1000)
            {
                // Modify the personality type

                result += buffData.buffName;
                _playerStatus.PersonalityType = result;
                if(_playerStatus.PersonalityType.Length == 4)
                    _playerStatus.InitPersonaValueAndStatus();
                personalityResults.SetText("It looks like your personality type is " + result + ". Let's see how you interact in a social setting.");
                return;
            }

            if (_dictBuffDuration.TryGetValue(buffData, out var value))
            {
                value.Add(buffData.duration);
                _playerStatus.InitPersonaValueAndStatus();
                //Debug.Log(_playerStatus.PersonalityType);
                //personalityResults.SetText("It looks like your personality type is " + result + ". Let's see how you interact in a social setting.");
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
            
            //_playerStatus.OnUpdateBuffEffect(buffData.trait, buffData.personaModifier, buffData.baseFavorabilityModifier);
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
            var debugStr = "";
            
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

                    var trait = buffData.trait;

                    var durationTime = durations[i];
                    debugStr +=
                        $"{buffData.buffName} - {trait} - {durationTime}\n";
                }
                
                Debug.LogWarning(debugStr);
            }
        }
    }
}
