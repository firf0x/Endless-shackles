using UnityEngine;
using Game.Lib;
using Game.Cards.Modifier;
using Game.GameSystem;
using System.Collections.Generic;

namespace Game.Cards
{
    public class ModifierDecorator : CardDecorator
    {
        private List<ModifierBase> modifiers;
        private PlayerSystem player;
        private IDeck<CardData> handDeck;
        private IDeck<CardData> defendDeck;
        private IDeck<CardData> monsterDeck;

        public ModifierDecorator(List<ModifierBase> modifiers, IDeck<CardData> handDeck, IDeck<CardData> defendDeck, IDeck<CardData> monsterDeck, PlayerSystem player, ICard<CardTypeEnum> card) : base(card)
        {
            this.player = player;
            this.handDeck = handDeck;
            this.defendDeck = defendDeck;
            this.monsterDeck = monsterDeck;
            this.modifiers = modifiers;
        }

        public void AddModifier(ModifierBase modifier)
        {
            if(modifier != null && !modifiers.Contains(modifier)) modifiers.Add(modifier);
        }

        public void RemoveModifier(ModifierBase modifier)
        {
            if(modifiers.Contains(modifier)) modifiers.Remove(modifier);
        }

        public override void Start()
        {
            // Первичная инициализация модификаторов
            foreach (var modifier in modifiers)
            {
                modifier.Init();
            }
        }

        public override void Use(GameObject target)
        {
            // Создание контекста с текущей картой в SourceCard
            var context = CreateContext(target);
            
            if(target != null)
            {
                // Нужен для отправки данных о карте взаимодействующей с текущей
                Debug.Log("Отправка обратной связи");
                target.GetComponent<CardData>().GetCardFeature<CallBackDecorator>().Call(Parent);
                Debug.Log("Конец обратной связи");
            }

            // Отработка всех модификаторов на текущей карте
            foreach (var modifier in modifiers)
            {
                modifier.Apply(context);
            }

            base.Use(target);
        }

        public void UpdateModifiers(GameObject target)
        {
            var context = CreateContext(target);

            foreach (var modifier in modifiers)
            {
                modifier.OnUpdate(context);
            }            
        }

        private ModifierContext CreateContext(GameObject target)
        {
            return new ModifierContext
            {
                //TODO: я так подумал и считаю, что CardData должена быть закеширована это сократит количество вызовов getcomponent
                SourceCard = this,
                TargetCard = target?.GetComponent<CardData>()?.decorateCard,
                TargetGameObject = target,
                Player = player,
                HandDeck = handDeck,
                DefendDeck = defendDeck,
                MonsterDeck = monsterDeck,
                DamageValue = Parent.GetComponent<CardData>().GetCardFeature<AttackDecorator>().currentDamage.Value
            };
        }

        public override string ToString()
        {
            string message = $"Modifier: текущее количество модификаторов {modifiers.Count}.";
            Debug.Log(message);
            return message;
        }

        public override void Dispose()
        {
            modifiers.Clear();
            modifiers = null;
            player = null;
            handDeck = null;
            defendDeck = null;
            monsterDeck = null;

            base.Dispose();
        }
    }
}