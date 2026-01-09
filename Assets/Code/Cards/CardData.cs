using Game.Lib;
using UnityEngine;

namespace Game.Cards
{
    public class CardData : MonoBehaviour
    {
        [SerializeField] public SpriteRenderer ObjectRenderer;
        public ICard<CardTypeEnum> decorateCard;

        public void Execute(GameObject target)
        {
            decorateCard?.Use(target);
        }

        public T GetCardFeature<T>() where T : class
        {
            return decorateCard as T;
        }
        
        public bool TryGetCardFeature<T>(out T feature) where T : class
        {
            feature = decorateCard as T;
            return feature != null;
        }
    }
}