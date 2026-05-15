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

        [field:SerializeField, Space(10f)] public float IdleVerticalAmplitude { get; private set; }
        [field:SerializeField] public float IdleVerticalMinDuration { get; private set; }
        [field:SerializeField] public float IdleVerticalMaxDuration { get; private set; }

        [field:SerializeField, Space(10f)] public float IdleRotationAngle { get; private set; }
        [field:SerializeField] public float IdleRotationMinDuration { get; private set; }
        [field:SerializeField] public float IdleRotationMaxDuration { get; private set; }

        [field:SerializeField, Space(10f)] public float DragFollowSmoothTime { get; private set;}
        [field:SerializeField] public float DragRotationAnimationDuration { get; private set;}
        [field:SerializeField] public float DragRotationAngle { get; private set;}
        [field:SerializeField] public float DragRotationFactor { get; private set;}
        [field:SerializeField] public float DragScale { get; private set;}
        [field:SerializeField] public float DragScaleAnimationDuration { get; private set;}


        [field:SerializeField, Space(10f)] public float ReturnAnimationDuration { get; private set; }
        [field:SerializeField] public Ease ReturnEase { get; private set; }

        // [field:SerializeField] public float IdleRotationMaxDuration { get; private set; }


    }
}