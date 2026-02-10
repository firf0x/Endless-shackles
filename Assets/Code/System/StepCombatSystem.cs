using System;
using UnityEngine;

namespace Game.GameSystem
{
    public class StepCombatSystem
    {
        public event Action EventUpdate;

        public void StepUpdate()
        {
            EventUpdate?.Invoke();
        }
    }
}