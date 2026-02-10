using System;
using UnityEngine;

namespace Game.GameSystem
{
    public class StepCombatSystem : MonoBehaviour
    {
        public static StepCombatSystem Instance { get; private set; }
        public event Action EventUpdate;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
        }

        public void StepUpdate()
        {
            Debug.Log("Update step");
            EventUpdate?.Invoke();
        }

        private void OnDestroy()
        {
            EventUpdate = null;
            // Instance = null;
        }
    }
}