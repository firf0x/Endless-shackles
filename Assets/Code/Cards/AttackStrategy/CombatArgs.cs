using UnityEngine;

namespace Game.Cards.Strategy
{
    public class CombatArgs
    {
        public readonly GameObject Parent;
        public readonly GameObject Target;
        public readonly int Damage;

        public CombatArgs(GameObject Parent, GameObject target, int Damage)
        {
            this.Parent = Parent;
            this.Target = target;
            this.Damage = Damage;
        }
    }
}