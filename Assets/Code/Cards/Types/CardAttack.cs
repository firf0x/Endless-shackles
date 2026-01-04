using System;
using Lib;
using UnityEngine;
using VContainer;

namespace Game.Cards
{
    [Serializable]
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