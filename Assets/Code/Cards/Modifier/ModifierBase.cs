using System;
using Game.Cards.Modifier;
using UnityEngine;

namespace Game.Lib
{    
    public abstract class ModifierBase : ScriptableObject
    {
        [field:SerializeField] public string modifierName { get; private set; }
        [field:SerializeField] public string modifierDescription { get; private set; }
        [field:SerializeField] public ModifierTypeEnum modifierType { get; private set; }
        [field:SerializeField] public Sprite icon { get; private set; }
        [field:SerializeField] public bool isStack { get; private set; } // TODO: нужно либо задать вопрос или просто закоментировать

        public virtual void Apply(ModifierContext context) { }
        public virtual void OnUpdate(ModifierContext context) { }
        public virtual void OnCallBack(ModifierContext context) { }
        public virtual void Init(ModifierContext context) { }
    }
}