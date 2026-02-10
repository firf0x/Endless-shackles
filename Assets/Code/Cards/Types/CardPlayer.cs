using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Cards
{
    [Serializable]
    public class CardPlayer : CardParametrs
    {
        //? Этот класс является исключительно зоной с данными.

        public UnityEvent Event;
    }
}