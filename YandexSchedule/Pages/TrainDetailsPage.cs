using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using YandexSchedule.Extensions;

namespace YandexSchedule.Pages
{
	public class TrainDetailsPage : BasePage, IAbstractPage<TrainDetailsPage>
	{
		public TrainDetailsPage(IWebDriver driver) : base(driver)
		{
		}

		public TrainDetailsPage GetPage()
		{
			return LoadPage();
		}

		public TrainDetailsPage LoadPage()
		{
			if (!IsTrainDetailsPageOpened())
			{
				throw new XPathLookupException(
					"Произошла ошибка: не загрузилась страница с деталями поездки.");
			}

			return this;
		}

		#region Простые методы

		/// <summary>
		/// Получить из таблицы пункт назначения
		/// </summary>
		public string GetPointFromText()
		{
			CustomTestContext.WriteLine("Получить из таблицы пункт отправления");

			return GetElementText(By.XPath(POINT_FROM));
		}

		/// <summary>
		/// Получить из таблицы пункт назначения
		/// </summary>
		public string GetPointToText()
		{
			CustomTestContext.WriteLine("Получить из таблицы пункт назначения");

			return GetElementText(By.XPath(POINT_TO));
		}

		/// <summary>
		/// Получить из таблицы пункт назначения
		/// </summary>
		public TimeSpan GetTripDuration()
		{
			CustomTestContext.WriteLine("Получить из таблицы пункт назначения");

			return GetTimeFromElement(By.XPath(DURATION), new string[] { @"h\ \ч\ mm\ \м\и\н", @"h\ \ч\ m\ \м\и\н" });
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		public bool IsTrainDetailsPageOpened()
		{
			return WaitUntilElementIsDisplay(By.XPath(HEADER));
		}

		/// <summary>
		/// Проверить, что на странице рейса есть информация о рейсе
		/// </summary>
		/// <param name="expectedText">ожидаемый текст</param>
		public bool IsHeaderContainsText(string expectedText)
		{
			CustomTestContext.WriteLine(
				$"Проверить, что заголовок таблицы содержит данные о рейсе: {expectedText}");

			return GetElementText(By.XPath(HEADER)).Contains(expectedText);
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = HEADER)]
		protected IWebElement PageHeader { get; set; }

		#endregion

		#region Описание XPath элементов

		private const string HEADER = "//h1[@class='b-page-title__title']";

		private const string POINT_FROM = "//tr[contains(@class, 'start')]//div[contains(@class, 'city')]//a";
		private const string POINT_TO = "//tr[contains(@class, 'end')]//div[contains(@class, 'city')]//a";
		private const string DURATION = "//tr[contains(@class, 'end')]//td[contains(@class,'cell_position_last')]//div[contains(@class, 'pathtime')]";

		#endregion
	}
}
