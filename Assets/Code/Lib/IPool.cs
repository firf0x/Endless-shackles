using System.Collections.Generic;
using UnityEngine;

namespace Lib
{
    public interface IPool<T>
    {
        T Get();
        void Release(T @object);
    }
}