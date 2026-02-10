using UnityEngine;
using Game.Lib;
using System;
using UnityEngine.UI;

namespace Game.Cards
{
    public class PlayerDecorator : CardDecorator, IDamageble
    {
        private Health healthSystem;
        private GameObject ui;

        public PlayerDecorator(GameObject GameOver, ICard<CardTypeEnum> card) : base(card)
        {
            this.healthSystem = new Health(1);
            ui = GameOver;
            ui.GetComponentInChildren<Button>().onClick.AddListener(ResetHealth);
        }

        public void TakeDamage(int amount)
        {
            healthSystem.TakeDamage(amount);
            // ToString();
            ui.SetActive(true);
        }

        private void ResetHealth()
        {
            healthSystem.Reset();
        }

        public override string ToString()
        {
            string message = $"Игрок умер.";
            Debug.Log(message);
            return message;
        }
    }
}