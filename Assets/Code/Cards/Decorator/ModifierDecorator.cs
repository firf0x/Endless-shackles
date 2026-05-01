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
        // public event Action<ModifierData> OnCleared;
        
        public IDeck<CardData> handDeck { get; private set; }
        public IDeck<CardData> defendDeck { get; private set; }
        public IDeck<CardData> monsterDeck { get; private set; }

        private List<ModifierData> modifiers;

        public ModifierDecorator(List<ModifierBase> modifiers, IDeck<CardData> handDeck, IDeck<CardData> defendDeck, IDeck<CardData> monsterDeck, ICard card) : base(card)
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
                if(!existing.Modifier.isStack || existing.Stack >= 99) return;
                existing.Stack += 1;
                OnChanged?.Invoke(existing);
            }
            else
            {
                ModifierData d = new ModifierData(modifier, 1);
                modifiers.Add(d);

                d.Modifier.Init(CreateContext(null, d));
                
                OnAdded?.Invoke(d);
            }
        }

        public void RemoveModifier(ModifierBase modifier)
        {
            if (modifier == null) return;

            ModifierData existing = modifiers.Find(m => m.Modifier == modifier);
            if (existing != null)
            {
                existing.Stack -= 1;
                if (existing.Stack <= 0 || !existing.Modifier.isStack)
                {
                    existing.Modifier.OnRemove(CreateContext(null, existing));
                    modifiers.Remove(existing);
                    
                    OnRemoved?.Invoke(existing);
                    OnChanged?.Invoke(existing);
                    
                    ScriptableObject.Destroy(existing.Modifier, 2f);
                    return;
                }
                OnChanged?.Invoke(existing);
            }
        }

        public bool HasModifier(ModifierBase mod) => modifiers.Exists(md => md.Modifier == mod);

        public ModifierBase GetModifier(ModifierBase modifier)
        {
            foreach (var modifierData in modifiers)
            {
                if (modifierData.Modifier == modifier) return modifierData.Modifier;
            }
            return null;
        }

        public override void Start()
        {
            base.Start();

            if(Parent.GetComponent<CardData>().TryGetCardFeature<StepCombatDecorator>(out var stepDecorator)) stepDecorator.OnStepEndUpdate += OnUpdate;
            StepCombatSystem.Instance.EventUpdate += OnGeneralUpdate;

            // Первичная инициализация модификаторов
            // foreach (var modifier in modifiers)
            // {
            //     modifier.Modifier.Init(CreateContext(null, modifier));
            // }
        }

        private void OnGeneralUpdate()
        {
            // if ( Parent.GetComponent<CardData>().TryGetCardFeature<StepCombatDecorator>(out var stepDecorator) && !stepDecorator.IsActive) Debug.Log(!stepDecorator.IsActive);
            if ( modifiers == null || modifiers.Count <= 0 || isLocked ) return;

            foreach (var modifier in modifiers.ToArray())
            {
                if(!modifier.Modifier.isIgnoring) modifier.Modifier.OnGeneralUpdate(CreateContext(null, modifier));
            }
        }

        private void OnUpdate()
        {
            if(isLocked) return;

            foreach (var modifier in modifiers.ToArray())
            {
                if(!modifier.Modifier.isIgnoring) modifier.Modifier.OnUpdate(CreateContext(null, modifier));
            }
        }

        public override void Use(GameObject target)
        {
            if(modifiers == null || modifiers.Count <= 0 || isLocked)
            {
                target.GetComponent<CardData>().GetCardFeature<CallBackDecorator>().Call(Parent);

                base.Use(target);
                return;
            }

            // Debug.Log($"{Parent.name}");
            // Debug.Log($"{target.name}");

            UpdateModifiers(target);
         
            // Нужен для отправки данных о карте взаимодействующей с текущей
            // Debug.Log("Отправка обратной связи");
            target.GetComponent<CardData>().GetCardFeature<CallBackDecorator>().Call(Parent);
            // Debug.Log("Конец обратной связи");

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
                SourceCard = this,
                SourceGameObject = Parent,
                SourceCardData = CardData,
                HandDeck = handDeck,
                DefendDeck = defendDeck,
                MonsterDeck = monsterDeck,
                DamageValue = CardData?.GetCardFeature<AttackDecorator>()?.currentDamage?.Value ?? 0,
                CurrentStackModifier = modifierData.Stack
            };
            else return new ModifierContext
            {
                SourceCard = this,
                SourceGameObject = Parent,
                SourceCardData = CardData,
                TargetCard = target?.GetComponent<CardData>()?.decorateCard,
                TargetGameObject = target,
                TargetCardData = target.GetComponent<CardData>(),
                HandDeck = handDeck,
                DefendDeck = defendDeck,
                MonsterDeck = monsterDeck,
                DamageValue = CardData?.GetCardFeature<AttackDecorator>()?.currentDamage?.Value ?? 0,
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
                if(modifier.Modifier != null) ScriptableObject.Destroy(modifier.Modifier, 2f);
            }

            modifiers.Clear();
            modifiers = null;
            handDeck = null;
            defendDeck = null;
            monsterDeck = null;

            OnAdded = null;
            OnRemoved = null;
            OnChanged = null;
            // OnCleared = null;

            if(Parent.GetComponent<CardData>().TryGetCardFeature<StepCombatDecorator>(out var stepDecorator)) stepDecorator.OnStepEndUpdate -= OnUpdate;
            StepCombatSystem.Instance.EventUpdate -= OnGeneralUpdate;

            base.Dispose();
        }
    }
}