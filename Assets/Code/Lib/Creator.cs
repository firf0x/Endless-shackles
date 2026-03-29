namespace Game.Lib
{
    public abstract class Creator<T>
    {
        public abstract T Create();
        public virtual T CreateAttackCard() => default;
        public virtual T CreateDefenceCard() => default;
    }
}