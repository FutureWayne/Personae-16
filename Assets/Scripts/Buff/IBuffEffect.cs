using UnityEngine;

namespace Buff
{
    public interface IBuffEffect
    {
        void AddBuff(GameObject target);
        void RemoveBuff(GameObject target);
    }
}
