using Game.GameSystem;
using Game.Lib;
using UnityEngine;

namespace Game.Cards.Modifier
{
    public class ModifierContext
    {
        public ICard<CardTypeEnum> SourceCard { get; set; }         // Карта-источник модификатора
        public ICard<CardTypeEnum> TargetCard { get; set; }         // Целевая карта
        public CardData SourceCardData { get; set; }
        public CardData TargetCardData { get; set; }
        public GameObject SourceGameObject { get; set; }            // Истоник игрового объекта
        public GameObject TargetGameObject { get; set; }            // Целевой игровой объект
        public int DamageValue { get; set; }                        // Значение урона
        public int CurrentStackModifier { get; set; }               // Значение стака текущего модификатора
        public PlayerSystem Player { get; set; }                    // Ссылка на игрока
        public IDeck<CardData> HandDeck { get; set; }               // Колода в руке
        public IDeck<CardData> DefendDeck { get; set; }             // Колода защиты
        public IDeck<CardData> MonsterDeck { get; set; }            // Колода монстров
    }
}