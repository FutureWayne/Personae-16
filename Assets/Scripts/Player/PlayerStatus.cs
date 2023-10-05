using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

public enum ECharacterStatusType
{
    Sociability,        //E
    Reflectiveness,     //I
    Practicality,       //S
    Imagination,        //N
    Analyticality,      //T
    Empathy,            //F
    Decisiveness,       //J
    Adaptability,       //P
}

public enum EPersonalityTraits
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
        private readonly Dictionary<EPersonalityTraits, int> _dictPlayerPersona = new();
        private readonly Dictionary<EPersonalityTraits, int> _dictCrushPersona = new();
        private readonly Dictionary<ECharacterStatusType, int> _dictPlayerStatus = new();
        
        [SerializeField]
        private string _playerPersonalityType;  // 4 letters, e.g. "INTP"
        [SerializeField]
        private string _crushPersonalityType;  // 4 letters, e.g. "INTP"
        
        private float _crushFavorability;
        
        
        private Dictionary<EPersonalityTraits, List<ECharacterStatusType>> _dictTrait2StatusType =
            new()
            {
                {
                    EPersonalityTraits.Mind,
                    new List<ECharacterStatusType> { ECharacterStatusType.Sociability, ECharacterStatusType.Reflectiveness }
                },                                                          // E/I
                {
                    EPersonalityTraits.Energy,
                    new List<ECharacterStatusType> { ECharacterStatusType.Practicality, ECharacterStatusType.Imagination }
                },                                                          // S/N              
                {
                    EPersonalityTraits.Nature,
                    new List<ECharacterStatusType> { ECharacterStatusType.Analyticality, ECharacterStatusType.Empathy }
                },                                                          // T/F                            
                {
                    EPersonalityTraits.Tactics,
                    new List<ECharacterStatusType> { ECharacterStatusType.Decisiveness, ECharacterStatusType.Adaptability }
                },                                                          // J/P                              
            };
        
        private void Awake()
        {
            //InitPersonaValueAndStatus();
        }
        
        public void InitPersonaValueAndStatus()
        {
            foreach (EPersonalityTraits trait in Enum.GetValues(typeof(EPersonalityTraits)))
            {
                if (trait is < EPersonalityTraits.Mind or > EPersonalityTraits.Tactics) continue;
                var personaTraitIndex = (int)trait;
                Debug.Log(personaTraitIndex.ToString());
                Debug.Log(_playerPersonalityType);
                var playerPersonaString = _playerPersonalityType[personaTraitIndex];
                var crushPersonaString = _crushPersonalityType[personaTraitIndex];

                var playerPersonaValueFactor = GetPersonaValueFactor(playerPersonaString);
                var crushPersonaValueFactor = GetPersonaValueFactor(crushPersonaString);

                _dictPlayerPersona.Add(trait, playerPersonaValueFactor * 3);
                _dictCrushPersona.Add(trait, crushPersonaValueFactor * 3);
                
                var playerStatusList = _dictTrait2StatusType[trait];
                foreach (var statusType in playerStatusList)
                {
                    _dictPlayerStatus.Add(statusType, 0);
                }
            }
            
            var playerPersonaLog = new StringBuilder();
            foreach (var pair in _dictPlayerPersona)
            {
                playerPersonaLog.AppendLine($"{pair.Key}: {pair.Value}");
            }
            Debug.LogError(playerPersonaLog.ToString());
        }

        private int GetPersonaValueFactor(char personaString)
        {
            return personaString switch
            {
                'E' => 1,
                'I' => -1,
                'S' => 1,
                'N' => -1,
                'T' => 1,
                'F' => -1,
                'J' => 1,
                'P' => -1,
                _ => 0
            };
        }

        private int GetPlayerStatusIndexByType(char personaString)
        {
            return personaString switch
            {
                'E' => 0,
                'I' => 1,
                'S' => 0,
                'N' => 1,
                'T' => 0,
                'F' => 1,
                'J' => 0,
                'P' => 1,
                _ => 0
            };
            
        }

        
        public string PersonalityType
        {
            get => _playerPersonalityType;
            set => _playerPersonalityType = value;
        }
        
        public int GetPlayerPersona(EPersonalityTraits trait)
        {
            return _dictPlayerPersona[trait];
        }
        
        public float GetCrushFavorability()
        {
            return _crushFavorability;
        }
        
        public int GetStatusByType(ECharacterStatusType statusType)
        {
            return _dictPlayerStatus[statusType];
        }

        // Update the player status and crush favorability and persona value by buff effect
        public void OnUpdateBuffEffect(EPersonalityTraits buffDataTrait, int buffDataPersonaModifier,
            int buffDataBaseFavorabilityModifier)
        {
            // 1. Get the index of affected player status
            var affectedPlayerStatusIndex = buffDataPersonaModifier > 0 ? 0 : 1;
            
            // 2. Get the affected player status type
            var affectedPlayerStatusType = _dictTrait2StatusType[buffDataTrait][affectedPlayerStatusIndex];
            
            // 3. Add the affected player status value
            _dictPlayerStatus[affectedPlayerStatusType] += 1;
            
            // 4. Change the player's persona value
            _dictPlayerPersona[buffDataTrait] += buffDataPersonaModifier;
            
            // 5. Get the crush persona value in that trait
            var crushPersonaValue = _dictCrushPersona[buffDataTrait];
            
            // 6. Get the player persona value in that trait
            var playerPersonaValue = _dictPlayerPersona[buffDataTrait];
            
            // 7. Get the choice factor (whether the choice aligns with the crush's personality, if yes, then 1, else -1)
            var choiceFactor = crushPersonaValue * buffDataPersonaModifier > 0 ? 1 : -1;
            
            // 8. Get the probability of winning the crush's favorability
            var probability = (-(crushPersonaValue + playerPersonaValue) + (3 * choiceFactor)) / 10f;
            
            // 9. Get the random number
            var randomNumber = Random.Range(0f, 1f);
            
            // 10. If the random number is larger than the probability, then the player doesn't win the favorability
            if (randomNumber > probability) return;
            
            // 11. Get the complementary status index ( find out which status is complementary to the crush's personality in this trait)
            var complementaryStatusIndex = crushPersonaValue > 0 ? 0 : 1;
            
            // 12. Get the complementary status type
            var complementaryStatusType = _dictTrait2StatusType[buffDataTrait][complementaryStatusIndex];
            
            // 13. Get the complementary status value of the player
            var complementaryStatusValue = _dictPlayerStatus[complementaryStatusType];
            
            // 14. Get the trait factor (the crush will have more favorability if the player has high complementary status in this trait)
            var traitFactor = (complementaryStatusValue / 10f) + 1;  
            
            // 15. Calculate the favorability value
            var favorabilityValue = (traitFactor * buffDataBaseFavorabilityModifier);
            
            // 16. Add the favorability value to the player
            _crushFavorability += favorabilityValue;
        }
    }
}
