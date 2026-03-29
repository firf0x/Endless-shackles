using Game.Lib;
using UnityEngine;

namespace Game.Cards.Modifier
{
    [CreateAssetMenu(fileName = "Corrosion modifier", menuName = "Modifiers/Corrosion")]
    public sealed class ModifierCorrosion : ModifierBase
    {
        public override void OnCallBack(ModifierContext context)
        {
            Debug.Log(ToString());
            
            if (context.SourceCardData.TryGetCardFeature<HealthDecorator>(out var decorator))
            {
                var attackDecorator = context.TargetGameObject.GetComponent<CardData>().GetCardFeature<AttackDecorator>();
                int currentDamage = attackDecorator.currentDamage.Value;
                decorator.TakeDamage(currentDamage);

                if(context.SourceCardData.TryGetCardFeature<ModifierDecorator>(out var modifierDecorator)) modifierDecorator.RemoveModifier(this);
            }
        }

        public override string ToString() => "Modifier corrosion";
    }
}