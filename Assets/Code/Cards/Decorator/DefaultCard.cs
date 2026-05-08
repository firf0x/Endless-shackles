using System.Collections.Generic;
using Game.Cards.Modifier;
using Game.Lib;
using UnityEngine;

namespace Game.Cards
{
    public class DefaultCard : ICard
    {
        public CardTypeEnum Type { get; set; }
        public CardTypeEnum IgnoreLayers { get; set; }
        public string CardName { get; set; }
        public string Description { get; set; }
        public bool isLocked { get; set; }
        public bool isDrag { get; set; } = true;
        public Sprite Icon { get; set; }
        public GameObject Parent { get; set; }
        public CardData CardData { get; set; }


        public void Start() { }
        public void Use(GameObject target) { }
        public void SetLock(bool enabled) => isLocked = enabled;
        public void SetDragActive(bool enabled) => isDrag = enabled;
        public void Destroy() => CardData.CardDestroy(true);

        public void Dispose()
        {
            Parent = null;
            // CardData = null;
        }
    }
}