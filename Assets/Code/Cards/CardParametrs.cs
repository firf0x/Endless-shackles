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
        [field:SerializeField] public CardTypeEnum Type { get; private set; }
        [field:SerializeField] public CardTypeEnum IgnoreLayers { get; private set; }
        [field:SerializeField] public string Name { get; private set; }
        [field:SerializeField, TextArea] public string Description { get; private set; }
        [field:SerializeField] public Sprite Icon { get; private set; }
        [field:SerializeField] public List<ModifierBase> Modifiers { get; private set; }
        [field:SerializeField, Range(0, 100)] public int Weight { get; private set; } = 1;
    }
}