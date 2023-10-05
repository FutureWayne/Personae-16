using System;
using UnityEngine;

namespace Buff
{
    [Serializable]
    public class BuffData : ScriptableObject, IBuffEffect
    {
        public int id;
        public string buffName;
        public EPersonalityAspects aspect;
        public int modifierA;
        public int modifierB;
        public int duration;
        public int personaModifier;
        
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
            return $"{id} - {buffName} - {aspect} - {modifierA} - {modifierB} - {duration} - {personaModifier}";
        }
    }
}

