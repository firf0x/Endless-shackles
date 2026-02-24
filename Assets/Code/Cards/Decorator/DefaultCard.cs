using System.Collections.Generic;
using Game.Cards.Modifier;
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
        public GameObject Parent { get; set; }  //! При очистке нельзя убирать

        // public List<T> GetModifiers<T>() where T : ModifierBase
        // {
        //     List<T> result = new List<T>();
            
        //     foreach (var modifier in Modifiers)
        //     {
        //         if (modifier is T typedEffect)
        //         {
        //             result.Add(typedEffect);
        //         }
        //     }
            
        //     return result;
        // }

        // public T GetModifier<T>() where T : ModifierBase
        // {
        //     foreach (var modifier in Modifiers)
        //     {
        //         if (modifier is T typedEffect)
        //         {
        //             return typedEffect;
        //         }
        //     }

        //     return null;
        // }
        public void Use(GameObject target) { }

        public void Dispose()
        {
            // Modifiers.Clear();
        }
    }
}