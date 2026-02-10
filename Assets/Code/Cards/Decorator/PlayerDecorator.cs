using UnityEngine;
using Game.Lib;
using System;

namespace Game.Cards
{
    public class PlayerDecorator : CardDecorator, IDamageble
    {
        private Health healthSystem;

        public PlayerDecorator(Action subscribes, ICard<CardTypeEnum> card) : base(card)
        {
            this.healthSystem = new Health(1, subscribes);
        }

        public void TakeDamage(int amount)
        {
            healthSystem.TakeDamage(amount);
            ToString();
        }

        public override string ToString()
        {
            string message = $"Игрок умер.";
            Debug.Log(message);
            return message;
        }
    }
}