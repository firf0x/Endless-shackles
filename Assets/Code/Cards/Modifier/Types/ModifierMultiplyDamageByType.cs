using Game.Cards.Strategy;
using Game.Lib;
using UnityEngine;

namespace Game.Cards.Modifier
{
    [CreateAssetMenu(fileName = "Multiply damage by type modifier", menuName = "Modifiers/Multiply damage by type")]
    public sealed class ModifierMultiplyDamageByType : ModifierBase
    {
        private DefenceType defenceType;

        public override void Init()
        {
            int rand = Random.Range(0, (int)DefenceType.Shield);
            defenceType = (DefenceType)rand;
        }

        public override void Apply(ModifierContext context)
        {
            context.SourceCard.Parent.GetComponent<CardData>()
                .GetCardFeature<AttackDecorator>()
                .ChangeStrategy(new MultiplyDamageByTypeStrategy(context.Player, context.DamageValue, context.DefendDeck, defenceType));
        }
    }
}