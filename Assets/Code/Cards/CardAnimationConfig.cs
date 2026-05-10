using DG.Tweening;
using UnityEngine;

namespace Game.Cards.UI
{
    
    [CreateAssetMenu(fileName = "Card Animation Config", menuName = "Game/Configs/Card Animation Config", order = 0)]
    public sealed class CardAnimationConfig : ScriptableObject
    {
        [Header("Idle state")]
        // [field:SerializeField] public float IdleAnimationDuration { get; private set; }
        [field:SerializeField] public LayerMask InteractionLayers { get; private set; }

        [field:SerializeField] public float IdleVerticalAmplitude { get; private set; }
        [field:SerializeField] public float IdleVerticalMinDuration { get; private set; }
        [field:SerializeField] public float IdleVerticalMaxDuration { get; private set; }

        [field:SerializeField] public float IdleRotationAngle { get; private set; }
        [field:SerializeField] public float IdleRotationMinDuration { get; private set; }
        [field:SerializeField] public float IdleRotationMaxDuration { get; private set; }

        [field:SerializeField] public float FollowSmoothTime { get; private set;}

        [field:SerializeField] public float ReturnAnimationDuration { get; private set; }
        [field:SerializeField] public Ease ReturnEase { get; private set; }
        // [field:SerializeField] public float IdleRotationMaxDuration { get; private set; }


    }
}