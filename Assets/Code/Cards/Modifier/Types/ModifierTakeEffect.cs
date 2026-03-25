using System.Collections.Generic;
using Game.Lib;
using UnityEngine;

namespace Game.Cards.Modifier
{
    [CreateAssetMenu(fileName = "Take Effect modifier", menuName = "Modifiers/TakeEffect")]
    public sealed class ModifierTakeEffect : ModifierBase
    {
        [SerializeField] private bool isRandom;
        [SerializeField] private List<ModifierBase> AdditionalModifiers;
        public override void Apply(ModifierContext context)
        {
            if(isRandom)
            {
                if(AdditionalModifiers.Count == 0) return;

                int randomIndex = Random.Range(0, AdditionalModifiers.Count);
                ModifierBase selected = AdditionalModifiers[randomIndex];
                context.TargetCardData.GetCardFeature<ModifierDecorator>().AddModifier(selected);
                Debug.Log($"Добавленый на карту {context.TargetGameObject.name} модификатор => {selected}");
            }
            else
            {
                foreach (ModifierBase modifier in AdditionalModifiers) context.TargetCardData.GetCardFeature<ModifierDecorator>().AddModifier(modifier);
            }
            Debug.Log("Модификаторы добавлены на вражескую карту");
        }

        public override void OnUpdate(ModifierContext context)
        {
            if(context.TargetCard.IgnoreLayers.HasFlag(CardTypeEnum.Attack | CardTypeEnum.Defence)) return;
            Apply(context);
        }
    
        public override string ToString() => "Modifier take effect";
    }
}