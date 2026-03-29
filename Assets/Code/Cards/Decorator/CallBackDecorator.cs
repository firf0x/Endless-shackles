using UnityEngine;
using Game.Lib;
using System;

namespace Game.Cards
{
    public class CallBackDecorator : CardDecorator
    {
        public CallBackDecorator(ICard<CardTypeEnum> card) : base(card) { }
        
        /// <summary>
        /// Отправляет текущий объект в ModifierDecorator другой карты. Для ответных реакций карты с другой кратой.
        /// </summary>
        /// <param name="target">Карта являющейся инициатором вызова</param>
        public void Call(GameObject target)
        {
            // Debug.Log(target.name + " : Карта которая была инициатором вызова");
            // Debug.Log(Parent.name + " : Карта которая была провзаимодействованой");
            Debug.Log(Parent.GetComponent<CardData>());
            Debug.Log(Parent.GetComponent<CardData>().TryGetCardFeature<ModifierDecorator>(out var decorator));
            Debug.Log(target);
            if (target != null) Parent.GetComponent<CardData>().GetCardFeature<ModifierDecorator>().OnCallbackReceived(target);
        }
    }
}