using UnityEngine;
using Game.Lib;
using System;
using System.Collections.Generic;
using Game.Cards.Passive;
using Game.GameSystem;

namespace Game.Cards
{
    public class PassiveDecorator : CardDecorator, ICallbackReceiver
    {
        private List<PassivesBase> passives;

        private IDeck<CardData> handDeck;
        private IDeck<CardData> defendDeck;
        private IDeck<CardData> monsterDeck;

        public PassiveDecorator(List<PassivesBase> passives, IDeck<CardData> handDeck, IDeck<CardData> defendDeck, IDeck<CardData> monsterDeck, ICard card) : base(card)
        {
            this.passives = passives;
            this.handDeck = handDeck;
            this.defendDeck = defendDeck;
            this.monsterDeck = monsterDeck;

            if(CardData.TryGetCardFeature<StepCombatDecorator>(out var decorator)) decorator.OnStepInteraction += OnStep;
            StepCombatSystem.Instance.EventUpdate += GeneralUpdate;
        }

        public override void Start()
        {
            base.Start();

            if(passives == null || passives.Count <= 0) return;

            foreach (var passive in passives)
            {
                passive.Init(CreateContext(null));
            }
        }

        private void GeneralUpdate()
        {
            if(passives == null || passives.Count <= 0 || isLocked) return;

            foreach (var passive in passives)
            {
                passive.OnGeneralUpdate(CreateContext(null));
            }
        }

        public void OnStep()
        {
            if(passives == null || passives.Count <= 0 || isLocked) return;

            foreach (var passive in passives)
            {
                passive.OnUpdate(CreateContext(null));
            }
        }

        public void OnCallbackReceived(GameObject target)
        {
            if(passives == null || passives.Count <= 0 || isLocked) return;
            
            foreach (var passive in passives)
            {
                passive.OnCallBack(CreateContext(target));
            }
        }

        private PassiveContext CreateContext(GameObject target)
        {
            return new PassiveContext(handDeck, defendDeck, monsterDeck)
            {
                Parent = Parent,
                Target = target
            };
        }

        public override void Dispose()
        {
            if(CardData.TryGetCardFeature<StepCombatDecorator>(out var decorator)) decorator.OnStepInteraction -= OnStep;

            if(passives.Count > 0) foreach (var passive in passives) passive.OnRemove(CreateContext(null));
            passives.Clear();
            passives = null;

            decoratedCard = null;
            
            base.Dispose();
        }

    }
}