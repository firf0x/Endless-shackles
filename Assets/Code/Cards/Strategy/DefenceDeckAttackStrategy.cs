using Game.GameSystem;
using Game.Lib;
using UnityEngine;

namespace Game.Cards.Strategy
{
    public abstract class DefenceDeckAttackStrategy : StrategyAttackBase
    {
        protected PlayerSystem Player { get; }
        protected IDeck<CardData> DefenceDeck { get; }
        protected int DamageValue { get; private set; }

        protected DefenceDeckAttackStrategy(PlayerSystem player, int damageValue, IDeck<CardData> defenceDeck)
        {
            Player = player;
            DamageValue = damageValue;
            DefenceDeck = defenceDeck;
        }

        public override void Execute()
        {
            // Если нет карт защиты – игрок погибает
            if (DefenceDeck.GetCardCount() == 0)
            {
                HandleNoTarget();
                return;
            }

            bool targeted = false;
            foreach (var card in DefenceDeck.cardDatas)
            {
                if (card?.decorateCard == null) continue;
                if (IsTarget(card))
                {
                    ApplyDamage(card);
                    targeted = true;
                    break;
                }
            }

            if (!targeted) HandleNoTarget();
        }

        /// <summary>
        /// Определяет, должна ли данная карта быть целью атаки.
        /// </summary>
        protected virtual bool IsTarget(CardData card) => true;

        /// <summary>
        /// Наносит урон выбранной карте.
        /// </summary>
        protected virtual void ApplyDamage(CardData target)
        {
            if (target.TryGetCardFeature<HealthDecorator>(out var health)) health.TakeDamage(DamageValue);
        }

        /// <summary>
        /// Действие, если ни одна карта не подошла под условие IsTarget.
        /// По умолчанию – убить игрока.
        /// </summary>
        protected virtual void HandleNoTarget() => Player.Kill();

        public override void UpdateDamageValue(int newDamageValue) => DamageValue = newDamageValue;
    }
}