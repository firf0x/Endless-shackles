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
            
            // healthDecorator.healthSystem.HealPoints.OnChanged += UpdateStats;
            Debug.Log(parent.GetComponent<CardData>().decorateCard.CardName);
        }

        public override void OnRemove()
        {
            // if(healthDecorator != null & healthDecorator.healthSystem != null) healthDecorator.healthSystem.HealPoints.OnChanged -= UpdateStats;
        }

        public void UpdateStats(int hp)
        {
            int bonus = hp / multiplicity;

            // Debug.Log(attackDecorator.Parent.transform.position + " : pos объекта");

            attackDecorator.ChangeDamage(bonus);
            stepDecorator.ChangeLimits(bonus);
        }
    }

    #endregion
}