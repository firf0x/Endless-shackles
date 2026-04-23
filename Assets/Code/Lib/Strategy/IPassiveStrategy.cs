using UnityEngine;

namespace Game.Lib
{
    public interface IPassiveStrategy
    {
        void Init(GameObject parent);
        void Apply(GameObject parent);
        void OnUpdate(GameObject parent);
    }
}