using UnityEngine;

namespace Game.Lib
{
    public interface IEffectHandler<T>
    {
        bool Apply(T data);
    }
}