using System.Collections.Generic;
using Game.Cards.Strategy;
using Game.GameSystem;
using Game.Lib;
using NUnit.Framework;
using UnityEngine;

namespace Game.Cards
{
    public class AttackDecorator : CardDecorator
    {
        public readonly int defaultDamageValue;
        public ReactiveProperty<int> currentDamage { get; private set; } = new();
        private PlayerSystem player;
        private IDeck<CardData> defenceDeck;
        private StrategyHandler<StrategyAttackBase> strategyHandle;

        public AttackDecorator(int damageValue, PlayerSystem player, IDeck<CardData> deck, ICard<CardTypeEnum> card) : base(card)
        {
            // currentDamage = new();
            this.defaultDamageValue = damageValue;
            ChangeDamage(0); // установка для того чтобы defaultDamageValue применился

            defenceDeck = deck;
            this.player = player;
        }

        public override void Start()
        {
            base.Start();
            strategyHandle = new StrategyHandler<StrategyAttackBase>(CreateStrategyByType());
        }

        private StrategyAttackBase CreateStrategyByType()
        {
            if (Type.HasFlag(CardTypeEnum.Monster))
            {
                Parent.GetComponent<CardData>().GetCardFeature<StepCombatDecorator>().OnStepInteraction += OnStep;
                return new MonsterAttackStrategy(
                    player: player,
                    damageValue: currentDamage.Value,
                    defenceDeck
                );

            }
            else
            {
                // Стандартная стратегия атаки
                return new StandartAttackStategy(
                    target: null,
                    parent: Parent,
                    damageValue: currentDamage.Value,
                    ignoreLayers: IgnoreLayers
                );
            }
        }

        private void OnStep() => Use(null);

        public override void Use(GameObject target)
        {
            base.Use(target);
            UpdateStrategyParameters(target);
            strategyHandle.ExecuteStrategy();
        }

        private void UpdateStrategyParameters(GameObject target)
        {
            strategyHandle.Strategy.UpdateDamageValue(currentDamage.Value);
            strategyHandle.Strategy.UpdateTarget(target);
        }

        public void ChangeDamage(int value)
        {
            currentDamage.Value = defaultDamageValue + value;
            currentDamage.Value = Mathf.Abs(currentDamage.Value);
        }

        public void ChangeStrategy(StrategyAttackBase newStrategy)
        {
            if (newStrategy == null)
            {
                Debug.LogError("Cannot change to null strategy");
                return;
            }
            
            strategyHandle.ChangeStrategy(newStrategy);
        }

        public override string ToString()
        {
            string message = $"Attack: было нанесено {currentDamage.Value} урона.";
            Debug.Log(message);
            return message;
        }

        public override void Dispose()
        {
            if (Parent.GetComponent<CardData>().TryGetCardFeature<StepCombatDecorator>(out var decorator)) decorator.OnStepInteraction -= OnStep;
            base.Dispose();
        }
    }
}