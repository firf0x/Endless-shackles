using Game.Lib;
using UnityEngine;

namespace Game.Cards.Modifier
{
    [CreateAssetMenu(fileName = "Corrosion modifier", menuName = "Modifiers/Corrosion")]
    public sealed class ModifierCorrosion : ModifierBase
    {
        private bool isActive;

        public override void Init()
        {
            isActive = true;
        }

        public override void Apply(ModifierContext context)
        {
            if(isActive == false) return;

            // Debug.Log(context.SourceCard.CardName);
            var a = context.SourceGameObject.GetComponent<CardData>().GetCardFeature<AttackDecorator>();

            a.ChangeDamage(a.currentDamage.Value);

            isActive = false;
        }

        public override void OnUpdate(ModifierContext context)
        {
            // Debug.Log(context.TargetCard.Parent.name);
        }

        private void OnDestroy()
        {
            
        }
    }
}