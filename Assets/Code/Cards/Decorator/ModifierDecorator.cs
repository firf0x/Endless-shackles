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
        
        public IDeck<CardData> handDeck { get; private set; }
        public IDeck<CardData> defendDeck { get; private set; }
        public IDeck<CardData> monsterDeck { get; private set; }

        private List<ModifierData> modifiers;

        public ModifierDecorator(List<ModifierBase> modifiers, IDeck<CardData> handDeck, IDeck<CardData> defendDeck, IDeck<CardData> monsterDeck, ICard<CardTypeEnum> card) : base(card)
        {
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
                if(!existing.Modifier.isStack) return;
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
                    existing.Modifier.OnRemove(CreateContext(null, existing));
                    modifiers.Remove(existing);
                    OnRemoved?.Invoke(existing);
                }
                OnChanged?.Invoke(existing);
            }
        }

        public bool HasModifier(ModifierBase mod) => modifiers.Exists(md => md.Modifier == mod);

        public override void Start()
        {
            // if(modifiers.Count <= 0 || isLocked) return;
            base.Start();

            if(Parent.GetComponent<CardData>().TryGetCardFeature<StepCombatDecorator>(out var stepDecorator)) stepDecorator.OnStepInteraction += OnStep;
            StepCombatSystem.Instance.EventUpdate += GeneralUpdate;

            // Первичная инициализация модификаторов
            foreach (var modifier in modifiers)
            {
                modifier.Modifier.Init(CreateContext(null, modifier));
            }
        }

        private void GeneralUpdate()
        {
            // if ( Parent.GetComponent<CardData>().TryGetCardFeature<StepCombatDecorator>(out var stepDecorator) && !stepDecorator.IsActive) Debug.Log(!stepDecorator.IsActive);
            if ( modifiers == null || modifiers.Count <= 0 || isLocked ) return;

            foreach (var modifier in modifiers.ToArray())
            {
                if(!modifier.Modifier.isIgnoring) modifier.Modifier.OnGeneralUpdate(CreateContext(null, modifier));
            }
        }

        public void OnStep()
        {
            if(modifiers == null || modifiers.Count <= 0 || isLocked) return;

            foreach (var modifier in modifiers.ToArray())
            {
                if(!modifier.Modifier.isIgnoring) modifier.Modifier.OnUpdate(CreateContext(null, modifier));
            }
        }

        public override void Use(GameObject target)
        {
            if(modifiers == null || modifiers.Count <= 0 || isLocked)
            {
                base.Use(target);
                return;
            }

            // Debug.Log($"{Parent.name}");
            // Debug.Log($"{target.name}");

            UpdateModifiers(target);
         
            CardData cardData = target.GetComponent<CardData>();
            
            // Нужен для отправки данных о карте взаимодействующей с текущей
            Debug.Log("Отправка обратной связи");
            cardData.GetCardFeature<CallBackDecorator>().Call(Parent);
            Debug.Log("Конец обратной связи");

            base.Use(target);
        }

        public void UpdateModifiers(GameObject target)
        {
            if(modifiers == null || modifiers.Count <= 0 || isLocked) return;

            foreach (var modifier in modifiers.ToArray())
            {
                if(!modifier.Modifier.isIgnoring) modifier.Modifier.OnUpdate(CreateContext(target, modifier));
            }
        }

        public void OnCallbackReceived(GameObject target)
        {
            if(modifiers == null || modifiers.Count <= 0 || isLocked) return;

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
            else sb.Append("Модификаторы отсутствуют.");

            string message = sb.ToString();
            Debug.Log(message);
            return message;
        }

        public override void Dispose()
        {
            foreach (var modifier in modifiers)
            {
                modifier.Modifier.OnRemove(CreateContext(null, modifier));
                ScriptableObject.DestroyImmediate(modifier.Modifier);
            }

            modifiers.Clear();
            modifiers = null;
            handDeck = null;
            defendDeck = null;
            monsterDeck = null;

            OnAdded = null;
            OnRemoved = null;
            OnChanged = null;
            OnCleared = null;

            if(Parent.GetComponent<CardData>().TryGetCardFeature<StepCombatDecorator>(out var stepDecorator)) stepDecorator.OnStepInteraction -= OnStep;
            StepCombatSystem.Instance.EventUpdate -= GeneralUpdate;

            base.Dispose();
        }
    }
}