using Game.Lib;
using UnityEngine;

namespace Game.Cards
{
    public class CardData : MonoBehaviour
    {
        [SerializeField] public SpriteRenderer ObjectRenderer;
        public IDeck<CardData> currentDeck;
        public ICard decorateCard;

        public void Execute(GameObject target)
        {
            decorateCard?.Use(target);
        }

        public T GetCardFeature<T>() where T : CardDecorator
        {
            var current = decorateCard;

            while (current != null)
            {
                if (current is T foundDecorator)
                {
                    return foundDecorator;
                }
                
                if (current is CardDecorator cardDecorator) current = cardDecorator.GetInnerCard();
                else current = null;
            }
            return null;
        }
        
        public bool TryGetCardFeature<T>(out T decorator) where T : CardDecorator
        {
            decorator = null;
            var current = decorateCard;

            while (current != null)
            {
                if (current is T foundDecorator)
                {
                    decorator = foundDecorator;
                    return true;
                }
                
                if (current is CardDecorator cardDecorator) current = cardDecorator.GetInnerCard();
                else current = null;
            }
            
            return false;
        }

        public bool CheckCardFeature<T>() where T : CardDecorator
        {
            var current = decorateCard;

            while (current != null)
            {
                if (current is T) return true;
                
                if (current is CardDecorator cardDecorator) current = cardDecorator.GetInnerCard();
                else current = null;
            }
            
            return false;
        }

        // private void OnDestroy()
        // {
        //     BreakDecoratorChain(decorateCard);
        //     decorateCard = null;
        // }

        // TODO: Нужно подумать о пуле объектов, так как я полностью очищаю декораторы и их связи. Что позволит мне задавать информацию полностью с нуля.

        private void BreakDecoratorChain(ICard card)
        {
            var currentCard = card;

            while (currentCard != null)
            {
                if (currentCard is CardDecorator decorator)
                {
                    var nextCard = decorator.GetInnerCard();
                    
                    // Debug.Log($"Удаление: {currentCard.GetType().Name}");

                    currentCard.Dispose();
                    currentCard = nextCard;
                }
                else
                {
                    // Debug.Log($"Окончание удаления: {currentCard.GetType().Name}");
                    currentCard.Dispose();
                    break;
                }
            }
        }

        public void CardDestroy(bool updateAllPosition)
        {
            BreakDecoratorChain(decorateCard);
            currentDeck.RemoveCard(this, updateAllPosition);
            // decorateCard.Parent = null;
            decorateCard = null;

            if (gameObject != null) Destroy(gameObject);
        }
    }
}