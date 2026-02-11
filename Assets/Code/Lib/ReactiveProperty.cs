using System;

namespace Game.Lib
{
    public class ReactiveProperty<T>
    {
        public event Action<T> OnChanged;

        private T value;

        public T Value
        {
            get => value;
            set
            {
                this.value = value;
                OnChanged?.Invoke(this.value);
            }
        }
    }
}