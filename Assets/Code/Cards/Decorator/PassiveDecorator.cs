using UnityEngine;
using Game.Lib;
using System;
using System.Collections.Generic;
using Game.Cards.Passive;
using Game.GameSystem;

namespace Game.Cards
{
    public class PassiveDecorator : CardDecorator
    {
        private List<PassivesBase> passives;

        public PassiveDecorator(List<PassivesBase> passives, IDeck<CardData> handDeck, IDeck<CardData> defendDeck, IDeck<CardData> monsterDeck, ICard<CardTypeEnum> card) : base(card)
        {
            this.passives = passives;
            StepCombatSystem.Instance.EventUpdate += OnStep;
        }

        public void OnStep()
        {
            if(passives.Count <= 0) return;

            foreach (var passive in passives)
            {
                passive.OnUpdate(Parent);
            }
        }

        public override void Start()
        {
            if(passives.Count <= 0) return;

            foreach (var passive in passives)
            {
                passive.Init(Parent);
            }

            base.Start();
        }

        public override void Dispose()
        {
            StepCombatSystem.Instance.EventUpdate -= OnStep;

            if(passives.Count > 0) foreach (var passive in passives) passive.OnRemove();
            passives.Clear();
            passives = null;

            decoratedCard = null;
            
            base.Dispose();
        }

    }
}