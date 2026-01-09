using UnityEngine;

namespace Game.Lib
{
    public interface IEffectHandler
    {
        bool Apply(GameObject target);
    }
}