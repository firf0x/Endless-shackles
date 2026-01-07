using System;
using Lib;
using UnityEngine;
using VContainer;

namespace Game.Cards
{
    [Serializable]
    public class CardDefence : CardParametrs
    {
        [SerializeField, InspectorName("Health")] public int HealthValue;
    }
}