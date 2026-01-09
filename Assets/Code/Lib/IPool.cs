using System.Collections.Generic;
using UnityEngine;

namespace Game.Lib
{
    public interface IPool<T>
    {
        T Get(Transform transform);
        void Release(T @object);
    }
}