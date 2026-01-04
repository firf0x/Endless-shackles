using Game.Cards;

namespace Game.Deck
{
    public interface IDeck
    {
        CardData[] cardDatas { get; }
        
        void AddCard(CardData newCard);
        void RemoveCard(CardData deletedCard);
    }
}