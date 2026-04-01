using System.Collections.Generic;
using Game.Cards.Modifier;
using Game.Lib;
using UnityEngine;

namespace Game.Cards
{
    public class DefaultCard : ICard<CardTypeEnum>
    {
        public CardTypeEnum Type { get; set; }  //!При очистке нельзя убирать
        public CardTypeEnum IgnoreLayers { get; set; } //! При очистке нельзя убирать
        public string CardName { get; set; }
        public string Description { get; set; }
        public bool isLocked { get; set; }
        public Sprite Icon { get; set; }
        public GameObject Parent { get; set; }  //! При очистке нельзя убирать

        public void Start() { }
        public void Use(GameObject target) { }
        public void SetLock(bool enabled) => isLocked = enabled;
        public void Destroy() => Parent.GetComponent<CardData>().CardDestroy(false);

        public void Dispose()
        {
            Parent = null;
        }
    }
}