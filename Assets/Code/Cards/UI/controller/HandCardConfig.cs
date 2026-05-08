using UnityEngine;

namespace Game.Cards.UI
{
    
    [CreateAssetMenu(fileName = "HandCardConfig", menuName = "Game/Configs/Hand card", order = 0)]
    public sealed class HandCardConfig : ScriptableObject
    {
        [Header("Idle state")]
        [field:SerializeField, InspectorName("Animation duration")] public float IdleAnimationDuration { get; private set; }
    }
}