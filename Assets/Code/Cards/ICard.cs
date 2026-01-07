using System.Collections.Generic;
using UnityEngine;
using Lib;

namespace Game.Cards
{
    public interface ICard
    {
        CardTypeEnum Type { get; }
        string CardName { get; }
        string Description { get; }
        Sprite Icon { get; }
        List<EffectBase> Effects { get; }
        
        void Use(GameObject target);
        List<T> GetEffects<T>() where T : EffectBase;
    }
}