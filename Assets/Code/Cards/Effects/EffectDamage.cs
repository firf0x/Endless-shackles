using System.Collections.Generic;
using Lib;
using UnityEngine;

namespace Game.Cards
{
    [CreateAssetMenu(fileName = "EffectDamage", menuName = "Game/CardEffect/EffectDamage", order = 0)]
    public class EffectDamage : EffectBase
    {
        [SerializeField] private int Damage;
        public override bool Apply(GameObject target)
        {
            CardData cardData = target.GetComponent<CardData>();

            List<EffectHealth> healthEffects = cardData.cardBase.GetEffects<EffectHealth>();
            
            if(healthEffects.Count == 0) return false;

            foreach (EffectHealth healthEffect in healthEffects)
            {
                ToString();
            }        
            return true;
        }

        public override string ToString()
        {
            string message = $"Attack: было нанесено {Damage} урона.";
            Debug.Log(message);
            return message;
        }
    }
}