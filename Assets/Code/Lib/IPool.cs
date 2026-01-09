using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Lib
{
    public interface IPool<T> : IDisposable
    {
        T Get(Transform transform);
        void Release(T @object);
    }
}