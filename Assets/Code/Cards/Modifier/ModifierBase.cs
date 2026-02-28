using System;
using Game.Cards.Modifier;
using UnityEngine;

namespace Game.Lib
{    
    public abstract class ModifierBase : ScriptableObject
    {
        [SerializeField] private string modifierName;
        [SerializeField] private string modifierDescription;
        [SerializeField] private ModifierTypeEnum modifierType;
        [SerializeField] private Sprite icon;

        public string Name => modifierName;
        public string Description => modifierDescription;
        public ModifierTypeEnum Type => modifierType;
        public Sprite Icon => icon;

        public virtual void Apply(ModifierContext context) { }
        public virtual void OnUpdate(ModifierContext context) { }
        public virtual void Init() { }
    }
}