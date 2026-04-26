using Game.Cards.Modifier;
using Game.Lib;
using UnityEngine;

namespace Game.Cards.Strategy
{
    public abstract class ModifierStrategyBase : ScriptableObject
    {
        public abstract string Name { get; }

        public abstract void Execute();
    }
}