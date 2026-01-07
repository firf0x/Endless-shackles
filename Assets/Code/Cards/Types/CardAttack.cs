using System;
using System.Collections.Generic;
using Lib;
using UnityEngine;
using VContainer;

namespace Game.Cards
{
    [Serializable]
    public class CardAttack : CardParametrs
    {
        [SerializeField, InspectorName("Damage")] public int DamageValue;
    }
}