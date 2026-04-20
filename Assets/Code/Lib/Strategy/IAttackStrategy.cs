using UnityEngine;

namespace Game.Lib
{
    public interface IAttackStrategy
    {
        string Name { get; }
        void Init();
        void Execute(GameObject parent, GameObject target, int damage);
        void OnRemove();
    }
}