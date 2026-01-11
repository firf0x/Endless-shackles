using Game.Lib;
using UnityEngine;

namespace Game.Cards
{
    public class CardData : MonoBehaviour
    {
        [SerializeField] public SpriteRenderer ObjectRenderer;
        public IDeck<CardData> currentDeck;
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

        private void OnDestroy()
        {
            BreakDecoratorChain(decorateCard);
            decorateCard = null;
        }

        private void BreakDecoratorChain(ICard<CardTypeEnum> card)
        {
            while (card != null)
            {
                card.Parent = null;
                
                if (card is CardDecorator decorator)
                {
                    card = decorator.GetInnerCard();
                }
                else
                {
                    break;
                }
            }
        }

        public void CardDestroy()
        {
            BreakDecoratorChain(decorateCard);
            decorateCard = null;
            
            if (gameObject != null) Destroy(gameObject);
        }
    }
}