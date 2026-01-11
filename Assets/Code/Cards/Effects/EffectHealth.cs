using Game.Lib;
using UnityEngine;

namespace Game.Cards
{
    [CreateAssetMenu(fileName = "EffectHealth", menuName = "Game/CardEffect/EffectHealth", order = 1)]
    public class EffectHealth : EffectBase<ICard<CardTypeEnum>>
    {
        // [SerializeField] private Health health;
        // public IDamageble HealthSystem => health;

        public override bool Apply(ICard<CardTypeEnum> card) => true;
    }
}