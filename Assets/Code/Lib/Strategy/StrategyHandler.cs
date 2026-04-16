using UnityEngine;

namespace Game.Lib
{
	/// <summary>
	/// Обработчик стратегий, предоставляющий механизм для выполнения и переключения между различными стратегиями
	/// </summary>
	public class StrategyHandler<TStrategy> : IStrategyHandle<TStrategy> where TStrategy : IAttackStrategy
	{
		/// <summary>
        /// Текущая стратегия
        /// </summary>
		public TStrategy Strategy { get; private set; }

        /// <summary>
        /// Создает экземпляр StrategyHandler с первоначальной стратегией
        /// </summary>
        /// <param name="startStrategy">Стратегия, которая будет установлена по умолчанию. Не может быть null</param>
        /// <exception cref="ArgumentNullException">Выбрасывается, если StartStrategy равен null</exception>
        public StrategyHandler(TStrategy startStrategy) => ChangeStrategy(startStrategy);

        /// <summary>
        /// Изменяет текущую стратегию на указанную
        /// </summary>
        /// <param name="newStrategy">Новая стратегия для установки</param>
        public void ChangeStrategy(TStrategy newStrategy)
		{
			if (newStrategy == null)
			{
				Debug.LogError($"Strategy cannot be null. Please provide a valid implementation of {typeof(TStrategy).Name}.");
				return;
			}
			Strategy = newStrategy;
			Strategy.Init();
		}

		/// <summary>
		/// Выполняет текущую установленную стратегию
		/// </summary>
		public void ExecuteStrategy(GameObject parent, GameObject target, int damage)
		{
			if (Strategy == null)
			{
				Debug.LogError($"Strategy cannot be null. Please provide a valid implementation of {typeof(TStrategy).Name}.");
				return;
			}
			Strategy.Execute(parent, target, damage);
		}

		/// <summary>
		/// Возвращает строковое представление текущей стратегии
		/// </summary>
		/// <returns>Название текущей стратегии</returns>
		public override string ToString() => $"Current strategy : {Strategy.Name}";
	}

	public interface IStrategyHandle<TStrategy>
	{
		/// <summary>
		/// Выполняет текущую установленную стратегию
		/// </summary>
		void ExecuteStrategy(GameObject parent, GameObject target, int damage);
		void ChangeStrategy(TStrategy newStrategy);
	}
}