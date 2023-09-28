using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public static class PersonalityHelper
    {
        private static readonly Dictionary<EPersonalityAspects, List<string>> DictPersonalityType =
            new Dictionary<EPersonalityAspects, List<string>>()
            {
                {EPersonalityAspects.Mind, new List<string>() {"I", "E"}},
                {EPersonalityAspects.Energy, new List<string>() {"N", "S"}},
                {EPersonalityAspects.Nature, new List<string>() {"F", "T"}},
                {EPersonalityAspects.Tactics, new List<string>() {"P", "J"}},
            };

        public static int GetAspectSideByType(EPersonalityAspects aspect, string type)
        {
            //Trim the type string by aspect
            
            var index = (int)aspect;
            var typeTrimmed = type[index].ToString();
            
            return DictPersonalityType[aspect].IndexOf(typeTrimmed);
        }
        
        public static ECharacterStatus GetStatusByAspect(EPersonalityAspects aspect)
        {
            return (ECharacterStatus) aspect;
        }
    }
}