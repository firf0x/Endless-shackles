using System;
using Game.Lib;
using UnityEngine;
using UnityEngine.Events;

namespace Game.GameSystem
{
    [Serializable]
    public class PlayerSystem : IDisposable
    {
        public static PlayerSystem Instance;
        public bool IsAlive { get; private set; } = true;
        public UnityEvent OnPlayerDied;

        public PlayerSystem() => Instance = this;

        public virtual void Kill()
        {
            if (!IsAlive) return;
            
            IsAlive = false;
            OnPlayerDied?.Invoke();
        }

        public virtual void Respawn()
        {
            if (!IsAlive) IsAlive = true;
        }

        public void Dispose()
        {
            if(Instance == this) Instance = null;
            OnPlayerDied?.RemoveAllListeners();
        }
    }
}