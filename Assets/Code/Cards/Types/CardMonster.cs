using Lib;
using UnityEngine;
using VContainer;

namespace Game.Cards
{
    [CreateAssetMenu(fileName = "Card Monster", menuName = "Game/Cards/Card Monster", order = 0)]
    public class CardMonster : CardBase
    {
        [SerializeField] private int Damage;
        [SerializeField] private int HP;

        private Health health;

        public override void Init()
        {
            throw new System.NotImplementedException();
        }

        public override void Use(GameObject target, GameObject caster)
        {
            target.GetComponent<IDamageble>().TakeDamage(Damage);
        }
    }
}