using System;
using System.Collections.Generic;
using Game.Lib;
using UnityEngine;

namespace Game.Cards
{
    [Serializable]
    public abstract class CardParametrs
    {
        [Header("Base Card Info")]
        [SerializeField] private CardTypeEnum type;
        [SerializeField] private CardTypeEnum ignoreLayers;
        [SerializeField] private string name;
        [SerializeField, TextArea] private string description;
        [SerializeField] private Sprite icon;
        [SerializeField] private List<ModifierBase> modifiers;

        public CardTypeEnum Type => type;
        public CardTypeEnum IgnoreLayers => ignoreLayers;
        public string CardName => name;
        public string Description => description;
        public Sprite Icon => icon;
        public List<ModifierBase> Modifiers => modifiers;
    }
}