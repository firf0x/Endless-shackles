using UnityEngine;

namespace Game.Lib
{
    public interface IModifierStrategy
    {
        string Name { get; }
        void Execute(GameObject parent, GameObject target);
    }
}