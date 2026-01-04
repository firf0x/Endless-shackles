using Lib;
using UnityEngine;
using VContainer;

namespace Game.Cards
{
    [CreateAssetMenu(fileName = "Card Defence", menuName = "Game/Cards/Card Defence", order = 0)]
    public class CardDefence : CardBase
    {
        [SerializeField] private Health Defence;
        public override void Init()
        {
            
        }
    }
}