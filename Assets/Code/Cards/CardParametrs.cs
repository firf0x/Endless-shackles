using System;
using System.Collections.Generic;
using Lib;
using UnityEngine;

namespace Game.Cards
{
    [Serializable]
    public abstract class CardParametrs
    {
        [Header("Base Card Info")]
        [SerializeField] private CardTypeEnum type;
        [SerializeField] private string cardName;
        [SerializeField, TextArea] private string description;
        [SerializeField] private Sprite icon;
        [SerializeField] private List<EffectBase> effects;


        public CardTypeEnum Type => type;
        public string CardName => cardName;
        public string Description => description;
        public Sprite Icon => icon;
        public List<EffectBase> Effects => effects;

        // public virtual void Use(GameObject target)
        // {
        //     foreach (IEffectHandler effect in Effects)
        //     {
        //         effect.Apply(target);
        //     }
        // }

        // public List<T> GetEffects<T>() where T : EffectBase
        // {
        //     List<T> result = new List<T>();
            
        //     foreach (var effect in Effects)
        //     {
        //         if (effect is T typedEffect)
        //         {
        //             result.Add(typedEffect);
        //         }
        //     }
            
        //     return result;
        // }
    }
}