using System;
using UnityEngine;

namespace Game.Cards.Passive
{
    #region Monsters
    
    [Serializable]
    public class MonsterHpToAttackAndCoolDown : PassivesBase
    {
        [SerializeField] private int multiplicity;

        private HealthDecorator healthDecorator;
        private AttackDecorator attackDecorator;
        private StepCombatDecorator stepDecorator;

        public override void Init(GameObject parent)
        {
            healthDecorator = parent.GetComponent<CardData>().GetCardFeature<HealthDecorator>();
            attackDecorator = parent.GetComponent<CardData>().GetCardFeature<AttackDecorator>();
            stepDecorator = parent.GetComponent<CardData>().GetCardFeature<StepCombatDecorator>();
        }

        public override void OnUpdate(GameObject parent) => UpdateStats(healthDecorator.healthSystem.HealPoints.Value);

        public void UpdateStats(int hp)
        {
            int bonus = hp / multiplicity;

            attackDecorator.ChangeDamage(bonus);
            stepDecorator.ChangeLimits(bonus);
        }
    }

    #endregion
}