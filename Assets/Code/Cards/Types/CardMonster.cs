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

        public override void Init()
        {
            
        }

        public override void Use( GameObject target )
        {
            target.GetComponent<CardData>();
        }
    }
}