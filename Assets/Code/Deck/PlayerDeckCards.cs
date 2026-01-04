using Game.Cards;
using UnityEngine;

namespace Game.Deck
{
    public class PlayerDeckCards : MonoBehaviour
    {
        private CardData[] cardDatas;
        [SerializeField] private int SizeDeck;


        private void OnValidate()
        {
            SizeDeck = Mathf.Max(SizeDeck, 0);
        }
    }
}