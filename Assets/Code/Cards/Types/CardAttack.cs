using System;
using System.Collections.Generic;
using Lib;
using UnityEngine;
using VContainer;

namespace Game.Cards
{
    [Serializable]
    public class CardAttack : CardBase
    {
        [SerializeField] public float a;
        public override void Init()
        {
            
        }

        public override void Use(GameObject target)
        {
            List<EffectDamage> effectsDamage = GetEffects<EffectDamage>();

            foreach (var EDamage in effectsDamage)
            {
                EDamage.Apply(target);
            }

        }
    }
}