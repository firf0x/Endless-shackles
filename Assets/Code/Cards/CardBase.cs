using Lib;
using UnityEngine;

namespace Game.Cards
{
    public abstract class CardBase
    {
        [Header("Base Card Info")]
        [SerializeField] private CardTypeEnum type;
        [SerializeField] private string cardName;
        [SerializeField, TextArea] private string description;
        [SerializeField] private Sprite icon;
        [SerializeField] private Health system;


        public CardTypeEnum Type => type;
        public string CardName => cardName;
        public string Description => description;
        public Sprite Icon => icon;
        public Health System => system;

        public abstract void Init();
        public virtual void Use(GameObject target) { }
        
        public virtual string GetEffectDescription()
        {
            return description;
        }
    }
}