using System;
using Game.Deck;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Utils
{
    [Serializable]
    public sealed class InteractionSpawnStrategy : InteractionBase
    {
        [SerializeField] private DeckComponent deck;

        public override void InteractionPressed(InputAction.CallbackContext context)
        {
            deck.CreateNewCard(deck.transform);
        }
    }
}