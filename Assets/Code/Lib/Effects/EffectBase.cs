using UnityEngine;

namespace Lib
{    
    public abstract class EffectBase : ScriptableObject, IEffectHandler
    {
        [SerializeField] private string effectName;
        [SerializeField] private string effectDiscription;

        public string Name => effectName;
        public string Discription => effectDiscription;

        public abstract bool Apply(GameObject target);
    }
}