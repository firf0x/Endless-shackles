// using System;
using System.Collections.Generic;
using Game.Cards.Passive;
using Game.Cards.Strategy;
using Game.Lib;
using UnityEngine;
// using 

namespace Game.Cards
{
    [System.Serializable]
    public class CardAttack : CardParametrs
    {
        //? Этот класс является исключительно зоной с данными.
        [SerializeField, InspectorName("Damage")] public int DamageValue;
        [SerializeReference, SubclassSelector] public IAttackStrategy strategyAttack;
    }
}