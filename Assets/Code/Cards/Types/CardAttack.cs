using System;
using Lib;
using UnityEngine;
using VContainer;

namespace Game.Cards
{
    [Serializable]
    [CreateAssetMenu(fileName = "Card Attack", menuName = "Game/Cards/Card Attack", order = 0)]
    public class CardAttack : CardBase
    {
        public override void Init()
        {
            
        }

        public override void Use(GameObject target)
        {
            target.GetComponent<CardData>().cardBase.System.TakeDamage(this.System.Value);
        }
    }
}