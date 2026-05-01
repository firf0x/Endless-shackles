using System;
using Game.Cards.Modifier;
using UnityEngine;

namespace Game.Lib
{    
    [Serializable]
    public abstract class ModifierBase : ScriptableObject
    {
        [field:SerializeField] public string modifierName { get; private set; }
        [field:SerializeField] public string modifierDescription { get; private set; }
        [field:SerializeField] public ModifierTypeEnum modifierType { get; private set; }
        [field:SerializeField] public Sprite icon { get; private set; }
        [field:SerializeField] public bool isStack { get; private set; } = true;
        [field:SerializeField] public bool isIgnoring { get; private set; }

        public void SetIgnoring(bool active) => isIgnoring = active;

        public virtual void Init(ModifierContext context) { }
        public virtual void OnGeneralUpdate(ModifierContext context) { }
        public virtual void OnUpdate(ModifierContext context) { }
        public virtual void OnRemove(ModifierContext context) { }
        public virtual void OnCallBack(ModifierContext context) { }
    }
}