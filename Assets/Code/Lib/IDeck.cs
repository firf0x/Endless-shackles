using System.Collections.Generic;

namespace Game.Lib
{
    public interface IDeck<TDataType>
    {
        TDataType[] cardDatas { get; }
        
        bool AddCard(TDataType newCard);
        void RemoveCard(TDataType deletedCard, bool isClearAll);
        public int GetCardCount();
    }
}