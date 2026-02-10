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

        // TODO: Нужно подумать о пуле объектов, так как я полностью очищаю декораторы и их связи. Что поззволит мне задавать информацию полностью с нуля.

        private void BreakDecoratorChain(ICard<CardTypeEnum> card)
        {
            var currentCard = card;
            
            while (currentCard != null)
            {
                if (currentCard is CardDecorator decorator)
                {
                    var nextCard = decorator.GetInnerCard();
                    
                    currentCard.Dispose();
                    currentCard = nextCard;
                }
                else
                {
                    currentCard.Dispose();
                    currentCard = null;
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