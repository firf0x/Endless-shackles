namespace Game.Lib
{
	/// <summary>
    /// Базовый интерфейс для всех стратегий, определяющий выполнение алгоритмов
    /// </summary>
	public interface IStrategy
	{
		/// <summary>
        /// Уникальное название стратегии для идентификации и отладки
        /// </summary>
		string Name { get; }

		/// <summary>
        /// Выполняет основную логику стратегии
        /// </summary>
		void Execute();
	}
}
