using System;
using Lib;
using UnityEngine;
using VContainer;

namespace Game.Cards
{
    [Serializable]
    public class CardMonster : CardBase
    {
        [SerializeField] private int Damage;
        [SerializeField] private int HP;

        public override void Init()
        {
            
        }

        public override void Use( GameObject target )
        {
            target.GetComponent<CardData>();
        }
    }
}