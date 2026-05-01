using System;
using System.Collections.Generic;
using Game.Lib;
using UnityEngine;

namespace Game.Cards.Modifier
{
    [CreateAssetMenu(fileName = "ModifierRabies", menuName = "Modifier/ModifierRabies", order = 0)]
    public sealed class ModifierRabies : ModifierBase
    {
        public override void OnUpdate(ModifierContext context)
        {
            if (context.SourceCardData.TryGetCardFeature<AttackDecorator>(out var attackDecorator))
            {
                var deck = context.MonsterDeck;

                if(deck.GetCardCount() == 1)
                {
                    var defenceCard = context.DefendDeck.cardDatas[UnityEngine.Random.Range(0, context.DefendDeck.GetCardCount())];
                    attackDecorator.ChangeTarget(defenceCard.gameObject);
                    return;
                }

                foreach (var card in deck.cardDatas)
                {
                    if(card == context.SourceCardData) continue;
                    else
                    {
                        attackDecorator.ChangeTarget(card.gameObject);
                        break;
                    }
                }
            }
        }
    }
}