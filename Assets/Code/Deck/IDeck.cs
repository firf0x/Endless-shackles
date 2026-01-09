using Game.Cards;

namespace Game.Deck
{
    public interface IDeck<TDataType>
    {
        TDataType[] cardDatas { get; }
        
        void AddCard(TDataType newCard);
        void RemoveCard(TDataType deletedCard);
    }
}