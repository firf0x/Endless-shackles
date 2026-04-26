using System;
using System.Collections.Generic;
using Game.Cards.Passive;
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
        [field:SerializeField, TextArea, Space(10f)] public string Description { get; private set; }
        [field:SerializeField, Space(10f)] public Sprite Icon { get; private set; }
        [field:SerializeField] public List<ModifierBase> Modifiers { get; private set; }
        [field:SerializeReference, SubclassSelector] public List<PassivesBase> Passives { get; private set; }
        [field:SerializeField, Range(0, 100), Space(25f)] public int Weight { get; private set; } = 1;
    }
}