using UnityEngine;
using Game.Lib;
using Game.Cards.Modifier;
using Game.GameSystem;
using System.Collections.Generic;
using System;

namespace Game.Cards
{
    public class ModifierDecorator : CardDecorator, ICallbackReceiver
    {
        public IReadOnlyList<ModifierData> Modifiers => modifiers;
        
        public event Action<ModifierData> OnAdded;
        public event Action<ModifierData> OnRemoved;
        public event Action<ModifierData> OnChanged;
        public event Action<ModifierData> OnCleared;

        private List<ModifierData> modifiers;
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
            this.modifiers = new();

            foreach (var modifier in modifiers)
            {
                AddModifier(modifier);
            }

        }

        public void AddModifier(ModifierBase modifier)
        {
            if (modifier == null) return;

            ModifierData existing = modifiers.Find(m => m.Modifier == modifier);

            if (existing != null)
            {
                if(existing.Modifier.isStack) return;
                existing.Stack += 1;
                OnChanged?.Invoke(existing);
            }
            else
            {
                ModifierData d = new ModifierData(modifier, 1);
                modifiers.Add(d);
                OnAdded?.Invoke(d);
            }
        }

        public void RemoveModifier(ModifierBase modifier)
        {
            if (modifier == null) return;

            var existing = modifiers.Find(m => m.Modifier == modifier);
            if (existing != null)
            {
                existing.Stack -= 1;
                if (existing.Stack <= 0)
                {
                    modifiers.Remove(existing);
                    OnRemoved?.Invoke(existing);
                }
                OnChanged?.Invoke(existing);
            }
        }

        public override void Start()
        {
            if(modifiers.Count == 0) return;
            if(Parent.GetComponent<CardData>().TryGetCardFeature<AttackDecorator>(out var a)) StepCombatSystem.Instance.EventUpdate += OnStep;

            // Первичная инициализация модификаторов
            foreach (var modifier in modifiers)
            {
                modifier.Modifier.Init();
            }
        }

        public void OnStep()
        {
            if(modifiers.Count <= 0) return;

            foreach (var modifier in modifiers.ToArray())
            {
                modifier.Modifier.Apply(CreateContext(null, modifier));
            }
        }

        public override void Use(GameObject target)
        {
            Debug.Log($"{Parent.name}");
            Debug.Log($"{target.name}");

            UpdateModifiers(target);

            CardData cardData = target.GetComponent<CardData>();

            if(!cardData.decorateCard.IgnoreLayers.HasFlag(CardTypeEnum.Attack))
            {
                cardData.GetCardFeature<ModifierDecorator>().UpdateModifiers(target);
            }
            else
            {
                // Нужен для отправки данных о карте взаимодействующей с текущей
                Debug.Log("Отправка обратной связи");
                cardData.GetCardFeature<CallBackDecorator>().Call(Parent);
                Debug.Log("Конец обратной связи");
            }

            base.Use(target);
        }

        public void UpdateModifiers(GameObject target)
        {
            if(modifiers.Count == 0) return;

            foreach (var modifier in modifiers.ToArray())
            {
                modifier.Modifier.OnUpdate(CreateContext(target, modifier));
            }
        }

        public void OnCallbackReceived(GameObject target)
        {
            if(modifiers.Count == 0) return;

            foreach (var modifier in modifiers.ToArray())
            {
                modifier.Modifier.OnCallBack(CreateContext(target, modifier));
            }
        }

        private ModifierContext CreateContext(GameObject target, ModifierData modifierData)
        {
            if(target == null) return new ModifierContext
            {
                //TODO: я так подумал и считаю, что CardData должена быть закеширована в сам ICard, это сократит количество вызовов getcomponent
                SourceCard = this,
                SourceGameObject = Parent,
                SourceCardData = Parent.GetComponent<CardData>(),
                Player = player,
                HandDeck = handDeck,
                DefendDeck = defendDeck,
                MonsterDeck = monsterDeck,
                DamageValue = Parent.GetComponent<CardData>()?.GetCardFeature<AttackDecorator>()?.currentDamage?.Value ?? 0,
                CurrentStackModifier = modifierData.Stack
            };
            else return new ModifierContext
            {
                //TODO: я так подумал и считаю, что CardData должена быть закеширована в сам ICard, это сократит количество вызовов getcomponent
                SourceCard = this,
                SourceGameObject = Parent,
                SourceCardData = Parent.GetComponent<CardData>(),
                TargetCard = target?.GetComponent<CardData>()?.decorateCard,
                TargetGameObject = target,
                TargetCardData = target.GetComponent<CardData>(),
                Player = player,
                HandDeck = handDeck,
                DefendDeck = defendDeck,
                MonsterDeck = monsterDeck,
                DamageValue = Parent.GetComponent<CardData>()?.GetCardFeature<AttackDecorator>()?.currentDamage?.Value ?? 0,
                CurrentStackModifier = modifierData.Stack
            };
        }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append($"Modifier: текущее количество модификаторов {modifiers.Count}. ");

            if (modifiers.Count > 0)
            {
                sb.Append("Список: ");
                for (int i = 0; i < modifiers.Count; i++)
                {
                    var modData = modifiers[i];
                    sb.Append($"[{modData.Modifier.ToString()}: стак {modData.Stack}]");
                    
                    if (i < modifiers.Count - 1) sb.Append(", ");
                }
            }
            else
            {
                sb.Append("Модификаторы отсутствуют.");
            }

            string message = sb.ToString();
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

            OnAdded = null;
            OnRemoved = null;
            OnChanged = null;
            OnCleared = null;

            if(Parent.GetComponent<CardData>().TryGetCardFeature<AttackDecorator>(out var a)) StepCombatSystem.Instance.EventUpdate -= OnStep;

            base.Dispose();
        }
    }
}