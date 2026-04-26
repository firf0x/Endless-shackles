using System;
using UnityEngine;

namespace Game.Lib
{
    /// <summary>
    /// Обработчик стратегий, работающий с любым типом стратегии и аргументов.
    /// </summary>
    /// <typeparam name="TStrategy">Тип стратегии, должен реализовывать IStrategy&lt;TArgs&gt;.</typeparam>
    /// <typeparam name="TArgs">Тип аргументов для выполнения.</typeparam>
    public class StrategyHandler<TStrategy, TArgs> : IDisposable, IStrategyHandle<TStrategy, TArgs> where TStrategy : IStrategy<TArgs>
    {
        public TStrategy Strategy { get; private set; }

        /// <param name="startStrategy">Начальная стратегия. Не может быть null.</param>
        /// <exception cref="ArgumentNullException">Если startStrategy == null.</exception>
        public StrategyHandler(TStrategy startStrategy)
        {
            if (startStrategy == null)
                throw new ArgumentNullException(nameof(startStrategy));
            ChangeStrategy(startStrategy);
        }

        public void ChangeStrategy(TStrategy newStrategy)
        {
            if (newStrategy == null)
            {
                Debug.LogError($"Strategy cannot be null. Provide a valid implementation of {typeof(TStrategy).Name}.");
                return;
            }

            Strategy?.OnRemove();
            Strategy = newStrategy;
            Strategy.Init();
        }

        public void ExecuteStrategy(TArgs args)
        {
            if (Strategy == null)
            {
                Debug.LogError($"Strategy cannot be null. Expected {typeof(TStrategy).Name}.");
                return;
            }
            Strategy.Execute(args);
        }

        public override string ToString() => $"Current strategy : {Strategy?.Name ?? "null"}";

        public void Dispose()
        {
            Strategy?.OnRemove();
        }
    }

    /// <summary>
    /// Контракт для обработчика стратегий.
    /// </summary>
    public interface IStrategyHandle<TStrategy, in TArgs>
        where TStrategy : IStrategy<TArgs>
    {
        void ExecuteStrategy(TArgs args);
        void ChangeStrategy(TStrategy newStrategy);
    }

	/// <summary>
    /// Базовый интерфейс стратегии с поддержкой инициализации, удаления и выполнения.
    /// </summary>
    /// <typeparam name="TArgs">Тип аргументов, передаваемых в Execute.</typeparam>
    public interface IStrategy<in TArgs>
    {
        string Name { get; }
        void Init();
        void OnRemove();
        void Execute(TArgs args);
    }
}