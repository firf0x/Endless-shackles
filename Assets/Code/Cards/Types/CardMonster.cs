using System;
using Lib;
using UnityEngine;
using VContainer;

namespace Game.Cards
{
    [Serializable]
    public class CardMonster : CardParametrs
    {
        //? Этот класс является исключительно зоной с данными.
        [SerializeField, InspectorName("Damage")] public int DamageValue;
        [SerializeField, InspectorName("Health")] public int HealthValue;
        [SerializeField, InspectorName("Step")] public int StepValue;
    }
}