using System;
using UnityEngine;

namespace Game.Cards
{
    [Serializable]
    public class CardDefence : CardParametrs
    {
        //? Этот класс является исключительно зоной с данными.
        [SerializeField, InspectorName("Health")] public int HealthValue;
    }
}