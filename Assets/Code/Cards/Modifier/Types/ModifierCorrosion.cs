using Game.Lib;
using UnityEngine;

namespace Game.Cards.Modifier
{
    [CreateAssetMenu(fileName = "Corrosion modifier", menuName = "Modifiers/Corrosion")]
    public sealed class ModifierCorrosion : ModifierBase
    {
        public override void OnUpdate(ModifierContext context)
        {
            var cardData = context.SourceCard.Parent.GetComponent<CardData>();

            Debug.Log(cardData.name);

            if (cardData.TryGetCardFeature<HealthDecorator>(out var decorator))
            {
                var attackDecorator = context.TargetGameObject.GetComponent<CardData>().GetCardFeature<AttackDecorator>();
                int currentDamage = attackDecorator.currentDamage.Value;
                
                decorator.TakeDamage(currentDamage);

                context.SourceGameObject.GetComponent<CardData>().GetCardFeature<ModifierDecorator>().RemoveModifier(this);
            }
        }
    }
}