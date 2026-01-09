using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Cards
{
    [Serializable]
    public class CardAttack : CardParametrs
    {
        //? Этот класс является исключительно зоной с данными.
        [SerializeField, InspectorName("Damage")] public int DamageValue;
    }
}