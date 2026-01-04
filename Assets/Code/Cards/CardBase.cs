using UnityEngine;

namespace Game.Cards
{
    public abstract class CardBase : ScriptableObject
    {
        [Header("Base Card Info")]
        [SerializeField] protected CardTypeEnum type;
        [SerializeField] protected string cardName;
        [SerializeField, TextArea] protected string description;
        [SerializeField] protected Sprite icon;
        
        public CardTypeEnum Type => type;
        public string CardName => cardName;
        public string Description => description;
        public Sprite Icon => icon;

        public abstract void Init();
        public virtual void Use(GameObject target, GameObject caster) { }
        
        public virtual string GetEffectDescription()
        {
            return description;
        }
    }
}