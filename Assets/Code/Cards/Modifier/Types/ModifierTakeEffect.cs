using System;
using System.Collections.Generic;
using Game.Lib;
using UnityEngine;

namespace Game.Cards.Modifier
{
    [CreateAssetMenu(fileName = "ModifierTakeEffect", menuName = "Modifier/ModifierTakeEffect", order = 0)]
    public sealed class ModifierTakeEffect : ModifierBase
    {
        [SerializeField] private bool isRandom;
        [SerializeField] private List<ModifierBase> AdditionalModifiers;
        public override void OnUpdate(ModifierContext context)
        {
            if(isRandom)
            {
                if(AdditionalModifiers.Count == 0) return;

                int randomIndex = UnityEngine.Random.Range(0, AdditionalModifiers.Count);
                ModifierBase selected = AdditionalModifiers[randomIndex];
                
                if(context.TargetCardData.TryGetCardFeature<ModifierDecorator>(out var decorator)) decorator.AddModifier(selected);
                
                Debug.Log($"Добавленый на карту {context.TargetGameObject.name} модификатор => {selected}");
            }
            else
            {
                foreach (ModifierBase modifier in AdditionalModifiers)
                {
                    context.TargetCardData.GetCardFeature<ModifierDecorator>().AddModifier(modifier);
                }
            }
            Debug.Log("Модификаторы добавлены на вражескую карту");
        }

        // public override void OnCallBack(ModifierContext context)
        // {
        //     Debug.Log(context.TargetCardData.decorateCard.CardName);
        //     OnUpdate(context);
        // }
    
        public override string ToString() => "Modifier take effect";
    }
}