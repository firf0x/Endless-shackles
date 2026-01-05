using Lib;
using UnityEngine;

namespace Game.Cards
{
    [CreateAssetMenu(fileName = "EffectHealth", menuName = "Game/CardEffect/EffectHealth", order = 1)]
    public class EffectHealth : EffectBase
    {
        [SerializeField] private Health health;
        public IDamageble HealthSystem => health;

        public override bool Apply(GameObject target) => true;

        public void OnDestroy()
        {
            health.Restart();
        }

        public override string ToString()
        {
            
            string message = $"Current HP: {health.Value}";
            Debug.Log(message);
            return message;
        }
    }
}