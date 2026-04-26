using Game.Cards.Passive;
using UnityEngine;

namespace Game.Lib
{
    public interface IPassiveStrategy
    {
        void Init(PassiveContext context);
        void OnGeneralUpdate(PassiveContext context);
        void OnUpdate(PassiveContext context);
    }
}