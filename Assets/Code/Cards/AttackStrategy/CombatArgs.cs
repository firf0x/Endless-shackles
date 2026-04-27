using UnityEngine;

namespace Game.Cards.Strategy
{
    public class CombatArgs
    {
        public readonly CardData Parent;
        public readonly CardData Target;
        public readonly int Damage;

        public CombatArgs(CardData Parent, CardData target, int Damage)
        {
            this.Parent = Parent;
            this.Target = target;
            this.Damage = Damage;
        }
    }
}