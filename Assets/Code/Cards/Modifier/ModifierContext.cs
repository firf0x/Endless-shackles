using System.Collections.Generic;
using Game.GameSystem;
using Game.Lib;
using UnityEngine;

namespace Game.Cards.Modifier
{
    public class ModifierContext
    {
        public ICard<CardTypeEnum> SourceCard { get; set; }         // Карта-источник модификатора
        public ICard<CardTypeEnum> TargetCard { get; set; }         // Целевая карта
        public GameObject SourceGameObject { get; set; }            // Истоник игрового объекта
        public GameObject TargetGameObject { get; set; }            // Целевой игровой объект
        public int DamageValue { get; set; }                        // Значение урона
        // public List<ICard<CardTypeEnum>> CardsInHand { get; set; }  // Карты в руке
        // public List<ICard<CardTypeEnum>> CardsOnBoard { get; set; } // Карты на столе
        public PlayerSystem Player { get; set; }                    // Ссылка на игрока
        public IDeck<CardData> HandDeck { get; set; }             // Колода в руке
        public IDeck<CardData> DefendDeck { get; set; }             // Колода защиты
        public IDeck<CardData> MonsterDeck { get; set; }            // Колода монстров
    }
}