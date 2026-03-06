using Game.Lib;
using UnityEngine;

namespace Game.Cards.Modifier
{
    [CreateAssetMenu(fileName = "Bleeding modifier", menuName = "Modifiers/Bleeding")]
    public sealed class ModifierBleeding : ModifierBase
    {
        [SerializeField] private int MaxStack;
        [SerializeField] private int Damage;
        private int currentStack;

        public override void Init()
        {
            currentStack = MaxStack;
        }

        public override void Apply(ModifierContext context)
        {
            var decorator = context.SourceCard.Parent.GetComponent<CardData>().GetCardFeature<HealthDecorator>();
            decorator.TakeDamage(Damage * currentStack);
        }

        public override void OnUpdate(ModifierContext context)
        {
            currentStack -= 1;
            currentStack = Mathf.Max(currentStack, 0);
        }
    }
}