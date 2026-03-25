using Game.Lib;
using UnityEngine;

namespace Game.Cards.Modifier
{
    [CreateAssetMenu(fileName = "Bleeding modifier", menuName = "Modifiers/Bleeding")]
    public sealed class ModifierBleeding : ModifierBase
    {
        [SerializeField] private int MaxStack;
        [SerializeField] private int Damage;

        public override void Apply(ModifierContext context)
        {
            var decorator = context.SourceCardData.GetCardFeature<HealthDecorator>();
            decorator.TakeDamage(Damage * context.CurrentStackModifier);
        }

        public override void OnUpdate(ModifierContext context)
        {
            context.SourceCardData.GetCardFeature<ModifierDecorator>().RemoveModifier(this);
        }

        public override string ToString() => "Modifier bleeding";
    }
}