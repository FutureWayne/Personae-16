using System;
using UnityEngine;

namespace Buff
{
    [Serializable]
    public class BuffData : ScriptableObject, IBuffEffect
    {
        public int id;
        public string buffName;
        public EPersonalityTraits trait;
        public int duration;
        public int personaModifier;
        public int baseFavorabilityModifier;
        
        
        public void AddBuff(GameObject target)
        {
            target.GetComponent<BuffReceiver>()?.AddBuff(this);
        }

        public void RemoveBuff(GameObject target)
        {
            target.GetComponent<BuffReceiver>()?.RemoveBuff(this);
        }
        
        public override string ToString()
        {
            return $"{id} - {buffName} - {trait} - {duration} - {personaModifier}";
        }
    }
}

