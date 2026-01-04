using Lib;
using UnityEngine;

namespace Game.Cards
{
    public class CardData : MonoBehaviour
    {
        [SerializeField] public CardBase cardBase;
    
        public void Execute( GameObject target )
        {
            cardBase.Use( target );
        }
    }
}