using UnityEngine;

namespace Object.Cards
{
    public abstract class CardBase : ScriptableObject
    {
        [SerializeField] private CardTypeEnum Type;
    }
}