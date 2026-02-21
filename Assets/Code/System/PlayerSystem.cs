using System;
using Game.Lib;
using UnityEngine;
using UnityEngine.Events;

namespace Game.GameSystem
{
    [Serializable]
    public class PlayerSystem : IDisposable
    {
        public bool IsAlive { get; private set; } = true;
        public UnityEvent OnPlayerDied;

        public void Kill()
        {
            if (!IsAlive) return;
            
            IsAlive = false;
            OnPlayerDied?.Invoke();
        }

        public void Respawn()
        {
            if (!IsAlive) IsAlive = true;
        }

        public void Dispose()
        {
            OnPlayerDied?.RemoveAllListeners();
        }
    }
}