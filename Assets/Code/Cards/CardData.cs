using Lib;
using UnityEngine;

namespace Game.Cards
{
    public class CardData : MonoBehaviour
    {
        [SerializeField] public SpriteRenderer ObjectRenderer;
        public CardBase cardBase;
    
        public void Execute( GameObject target )
        {
            cardBase.Use( target );
        }
    }
}