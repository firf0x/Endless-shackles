using UnityEngine;

namespace Game.Lib
{
    public abstract class Creator<T>
    {
        public abstract T Create();
        public abstract T Create(Transform transform);
        public virtual T CreateAttackCard() => default; //TODO: Убрать это при релизе
        public virtual T CreateDefenceCard() => default;//TODO: Убрать это при релизе
    }
}