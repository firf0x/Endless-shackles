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
        public Health Damage;

        public override void Init()
        {
        }

        public override void Use(GameObject target, GameObject caster)
        {
            target.GetComponent<IDamageble>().TakeDamage(Damage.Value);
        }
    }
}