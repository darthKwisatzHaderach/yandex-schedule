using System;
using System.Collections.Generic;
using System.Globalization;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using YandexSchedule.Extensions;
using YandexSchedule.Models;

namespace YandexSchedule.Pages
{
	public class ScheduleMainPage : BasePage, IAbstractPage<ScheduleMainPage>
	{
		public ScheduleMainPage(IWebDriver driver) : base(driver)
		{
		}

		public ScheduleMainPage GetPage()
		{
			throw new System.NotImplementedException();
		}

		public ScheduleMainPage LoadPage()
		{
			if (!IsYandexScheduleMainPageOpened())
			{
				throw new XPathLookupException(
					"Произошла ошибка: не загрузилась главная страница расписаний Yandex.");
			}

			return this;
		}

		#region Простые методы

		/// <summary>
		/// Заполнить поле 'Откуда'
		/// </summary>
		/// <param name="point">Пункт отправления</param>
		public ScheduleMainPage FillPointFromField(string point)
		{
			CustomTestContext.WriteLine($"Заполнить поле 'Откуда' значением {point}");
			PointFromInput.Clear();
			PointFromInput.SendKeys(point);

			return LoadPage();
		}

		/// <summary>
		/// Заполнить поле 'Куда'
		/// </summary>
		/// <param name="point">Пункт назначения</param>
		public ScheduleMainPage FillPointToField(string point)
		{
			CustomTestContext.WriteLine($"Заполнить поле 'Куда' значением {point}");
			PointToInput.Clear();
			PointToInput.SendKeys(point);

			return LoadPage();
		}

		/// <summary>
		/// Заполнить поле 'Дата'
		/// </summary>
		/// <param name="date">Дата</param>
		public ScheduleMainPage FillDateField(string date)
		{
			CustomTestContext.WriteLine($"Заполнить поле 'Дата' значением {date}");
			DateInput.Clear();
			DateInput.SendKeys(date);

			return LoadPage();
		}

		/// <summary>
		/// Заполнить поле 'Дата'
		/// </summary>
		/// <param name="date">Дата</param>
		public ScheduleMainPage FillDateField(DateTime date)
		{
			CustomTestContext.WriteLine($"Заполнить поле 'Дата' значением {date}");
			DateInput.SendKeys(date.ToString("dd/M/yyyy", CultureInfo.InvariantCulture));

			return LoadPage();
		}

		/// <summary>
		/// Нажать на кнопку 'Найти'
		/// </summary>
		public ScheduleMainPage ClickFindButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Найти'");
			FindButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на кнопку 'Электрички'
		/// </summary>
		public ScheduleMainPage ClickSuburbanTypeButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Электрички'");
			SuburbanTypeButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Открыть страницу с информацией о рейсе
		/// </summary>
		/// <param name="trainNumber">номер строки</param>
		public TrainDetailsPage OpenTrainDetails(string trainNumber)
		{
			CustomTestContext.WriteLine($"Открыть страницу с информацией о рейсе №{trainNumber}");
			RowLink = Driver.FindElement(By.XPath(ROW_LINK.Replace("*#*", trainNumber)));
			RowLink.Click();

			return new TrainDetailsPage(Driver).LoadPage();
		}

		#endregion

		#region Составные методы

		/// <summary>
		/// Выполнить поиск рейса
		/// </summary>
		/// <param name="pointFrom">пункт отправления</param>
		/// <param name="pointTo">пункт назначения</param>
		/// <param name="date">дата</param>
		public ScheduleMainPage Search(
			string pointFrom,
			string pointTo,
			DateTime date)
		{
			FillPointFromField(pointFrom);
			FillPointToField(pointTo);
			FillDateField(date);
			ClickFindButton();

			if (!IsSearchResultsDisplayed(pointFrom, pointTo, date))
			{
				throw new XPathLookupException("Ошибка: не отобразились результаты поиска");
			}

			return LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		public bool IsYandexScheduleMainPageOpened()
		{
			return WaitUntilElementIsDisplay(By.XPath(POINT_FROM));
		}

		public bool IsSearchResultsDisplayed(string pointFrom, string pointTo, DateTime dateTime)
		{
			CustomTestContext.WriteLine($"Проверить, что появились результаты" +
				$" поиска с параметрами '{pointFrom}', '{pointTo}', '{dateTime}'");

			return SearchResultHeader.Text.Contains(pointFrom) &&
			       SearchResultHeader.Text.Contains(pointTo) &&
			       (GetDateTimeFromElement(By.XPath(SEARCH_RESULT_DATE)) == dateTime);
		}

		#endregion

		#region Вспомогательные методы

		public List<Trip> ParseSearchResultsTable()
		{
			int rows = Driver.FindElements(By.XPath(ROW)).Count;
			List<Trip> trips = new List<Trip>(rows);

			for (int i = 1; i < (rows + 1); i++)
			{
				string[] points = GetElementText(By.XPath(ROW_TRAIN_TITLE.Replace("*#*", i.ToString()))).Split('—');
				DateTime startDateTime = GetDateTimeFromElement(By.XPath(SEARCH_RESULT_DATE)) +
				                    GetTimeFromElement(By.XPath(ROW_TRAIN_START_TIME.Replace("*#*", i.ToString())));
				DateTime endDateTime = GetDateTimeFromElement(By.XPath(SEARCH_RESULT_DATE)) +
				                  GetTimeFromElement(By.XPath(ROW_TRAIN_END_TIME.Replace("*#*", i.ToString())));

				trips.Add(new Trip(
					number: GetElementText(By.XPath(ROW_TRAIN_NUMBER.Replace("*#*", i.ToString()))),
					pointFrom: points[0].Trim(),
					pointTo: points[1].Trim(),
					startDateTime: startDateTime,
					duration: GetTimeFromElement(By.XPath(ROW_TRIP_DURATION.Replace("*#*", i.ToString())), new string[]{ @"h\ \ч\ mm\ \м\и\н", @"h\ \ч\ m\ \м\и\н" }),
					endDateTime: endDateTime,
					price: GetNumberFromElement(By.XPath(ROW_TRAIN_PRICE.Replace("*#*", i.ToString()))),
					currency: GetCurrencyFromElement(By.XPath(TABLE_CURRENCY))
					));
			}

			for (int i = trips.Count - 1; i >= 0; i--)
			{
				if ((trips[i].Number == string.Empty) && (trips[i].PointFrom == string.Empty))
				{
					trips.Remove(trips[i]);
				}
			}

			return trips;
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = POINT_FROM)]
		protected IWebElement PointFromInput { get; set; }

		[FindsBy(How = How.XPath, Using = POINT_TO)]
		protected IWebElement PointToInput { get; set; }

		[FindsBy(How = How.XPath, Using = DATE_INPUT)]
		protected IWebElement DateInput { get; set; }

		[FindsBy(How = How.XPath, Using = FIND_BUTTON)]
		protected IWebElement FindButton { get; set; }

		[FindsBy(How = How.XPath, Using = SUBURBAN_TYPE_BUTTON)]
		protected IWebElement SuburbanTypeButton { get; set; }

		[FindsBy(How = How.XPath, Using = SEARCH_RESULT_HEADER)]
		protected IWebElement SearchResultHeader { get; set; }

		[FindsBy(How = How.XPath, Using = SEARCH_RESULT_DATE)]
		protected IWebElement SearchResultDate { get; set; }

		protected IWebElement RowLink { get; set; }

		#endregion

		#region Описание XPath элементов

		private const string PAGE_URL = "https://rasp.yandex.ru/";
		private const string TITLE = "//title[contains(text(), 'Расписание самолётов, поездов, электричек и автобусов')]";

		private const string POINT_FROM = "//input[@name='fromName']";
		private const string POINT_TO = "//input[@name='toName']";
		private const string DATE_INPUT = "//input[@class='date-input_search__input']";
		private const string FIND_BUTTON = "//div[@class='search-form__submit']//button[not(contains(@class, 'disabled'))]";
		private const string SUBURBAN_TYPE_BUTTON = "//input[@value='suburban']/ancestor::label";

		private const string SEARCH_RESULT_HEADER = "//header[@class='SearchTitle']//h1/span";
		private const string SEARCH_RESULT_DATE = "//header[@class='SearchTitle']//span[@class='SearchTitle__subtitle']";

		private const string TABLE_CURRENCY = "//div[contains(@class, 'CurrencySelect')]//span[contains(@class, 'title')]";
		private const string ROW = "//article";
		private const string ROW_LINK = "//article//a[contains(@title, '*#*')]";
		private const string ROW_TRAIN_NUMBER = "(//article)[*#*]//span[contains(@class, 'number')]";
		private const string ROW_TRAIN_TITLE = "(//article)[*#*]//span[contains(@class, 'title')]";
		private const string ROW_TRAIN_START_TIME = "((//article)[*#*]//span[contains(@class, 'time')])[1]";
		private const string ROW_TRIP_DURATION = "(//article)[*#*]//div[contains(@class, 'duration')]";
		private const string ROW_TRAIN_END_TIME = "((//article)[*#*]//span[contains(@class, 'time')])[2]";
		private const string ROW_TRAIN_PRICE = "(//article)[*#*]//span[contains(@class, 'Price')]";

		#endregion
	}
}
