using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public static class PersonalityHelper
    {
        private static readonly Dictionary<EPersonalityTraits, List<string>> DictPersonalityType =
            new Dictionary<EPersonalityTraits, List<string>>()
            {
                {EPersonalityTraits.Mind, new List<string>() {"I", "E"}},
                {EPersonalityTraits.Energy, new List<string>() {"N", "S"}},
                {EPersonalityTraits.Nature, new List<string>() {"F", "T"}},
                {EPersonalityTraits.Tactics, new List<string>() {"P", "J"}},
            };

        public static int GetAspectSideByType(EPersonalityTraits trait, string type)
        {
            //Trim the type string by aspect
            
            var index = (int)trait;
            var typeTrimmed = type[index].ToString();
            
            return DictPersonalityType[trait].IndexOf(typeTrimmed);
        }
        
        public static ECharacterStatusType GetStatusByAspect(EPersonalityTraits trait)
        {
            return (ECharacterStatusType) trait;
        }
    }
}