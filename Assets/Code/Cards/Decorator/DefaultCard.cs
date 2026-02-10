using System.Collections.Generic;
using Game.Lib;
using UnityEngine;

namespace Game.Cards
{
    public class DefaultCard : ICard<CardTypeEnum>
    {
        public CardTypeEnum Type { get; set; }  //!При очистке нельзя убирать
        public CardTypeEnum IgnoreLayers { get; set; } //! При очистке нельзя убирать
        public string CardName { get; set; }
        public string Description { get; set; }
        public Sprite Icon { get; set; }
        public List<EffectBase<ICard<CardTypeEnum>>> Effects { get; set; }
        public GameObject Parent { get; set; }  //! При очистке нельзя убирать

        public List<T> GetEffects<T>() where T : EffectBase<ICard<CardTypeEnum>>
        {
            List<T> result = new List<T>();
            
            foreach (var effect in Effects)
            {
                if (effect is T typedEffect)
                {
                    result.Add(typedEffect);
                }
            }
            
            return result;
        }

        public T GetEffect<T>() where T : EffectBase<ICard<CardTypeEnum>>
        {
            foreach (var effect in Effects)
            {
                if (effect is T typedEffect)
                {
                    return typedEffect;
                }
            }

            return null;
        }
        public void Use(GameObject target) { }

        public void Dispose()
        {
            Effects.Clear();
        }
    }
}