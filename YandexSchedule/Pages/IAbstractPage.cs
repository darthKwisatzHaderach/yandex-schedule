namespace YandexSchedule.Pages
{
	public interface IAbstractPage<out T> where T : class
	{
		/// <summary>
		/// Переход на страницу
		/// </summary>
		T GetPage();

		/// <summary>
		/// Ожидание загрузки страницы
		/// </summary>
		T LoadPage();
	}
}
