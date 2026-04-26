using System;
using Game.Lib;
using UnityEngine;

namespace Game.Cards.Modifier
{
    [CreateAssetMenu(fileName = "ModifierBleeding", menuName = "Modifier/ModifierBleeding", order = 0)]
    public sealed class ModifierBleeding : ModifierBase
    {
        [SerializeField] private int MaxStack;
        [SerializeField] private int Damage;

        public override void OnUpdate(ModifierContext context)
        {
            var decorator = context.SourceCardData.GetCardFeature<HealthDecorator>();
            decorator.TakeDamage(Damage * context.CurrentStackModifier);
        }

        public override void OnCallBack(ModifierContext context)
        {
            context.SourceCardData.GetCardFeature<ModifierDecorator>().RemoveModifier(this);
        }

        public override string ToString() => "Modifier bleeding";
    }
}