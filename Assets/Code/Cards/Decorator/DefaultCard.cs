using System.Collections.Generic;
using Game.Lib;
using UnityEngine;

namespace Game.Cards
{
    public class DefaultCard : ICard<CardTypeEnum>
    {
        public CardTypeEnum Type { get; set; }
        public CardTypeEnum IgnoreLayers { get; set; }
        public IPool<ICard<CardTypeEnum>> Pool { get; set; }
        public string CardName { get; set; }
        public string Description { get; set; }
        public Sprite Icon { get; set; }
        public List<EffectBase> Effects { get; set; }
        public GameObject Parent { get; set; }

        public List<T> GetEffects<T>() where T : EffectBase
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

        public void OnReleaseToPool()
        {
            Effects.Clear();
        }

        public void Use(GameObject target) { }
    }
}