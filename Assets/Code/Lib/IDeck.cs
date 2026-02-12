using System.Collections.Generic;

namespace Game.Lib
{
    public interface IDeck<TDataType>
    {
        IReadOnlyList<TDataType> cardDatas { get; }
        
        void AddCard(TDataType newCard);
        void RemoveCard(TDataType deletedCard, bool isClearAll);
        void UpdateAllCardsPosition();
        public int GetCardCount();
    }
}