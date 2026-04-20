using System.Collections.Generic;

namespace Game.Lib
{
    public interface IDeck<DataType>
    {
        DataType[] cardDatas { get; }
        
        bool AddCard(DataType newCard);
        void RemoveCard(DataType deletedCard, bool isClearAll);
        public int GetCardCount();
    }
}