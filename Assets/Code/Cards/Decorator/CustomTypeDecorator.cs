using UnityEngine;
using Game.Lib;
using System;

namespace Game.Cards
{
    public class CustomTypeDecorator<TEnum> : CardDecorator where TEnum : Enum
    {
        public readonly TEnum CustomType;

        public CustomTypeDecorator(TEnum type, ICard<CardTypeEnum> card) : base(card)
        {
            CustomType = type;
        }

        public override string ToString()
        {
            string message = $"Enum: текущий тип {CustomType}.";
            Debug.Log(message);
            return message;
        }
    }
}