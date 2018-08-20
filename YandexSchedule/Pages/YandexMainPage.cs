using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using YandexSchedule.Extensions;

namespace YandexSchedule.Pages
{
	class YandexMainPage : BasePage, IAbstractPage<YandexMainPage>
	{
		public YandexMainPage(IWebDriver driver) : base(driver)
		{
		}

		public YandexMainPage GetPage()
		{
			Driver.Navigate().GoToUrl(PAGE_URL);

			return LoadPage();
		}

		public YandexMainPage LoadPage()
		{
			if (!IsYandexMainPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка: не загрузилась главная страница Yandex.");
			}

			return this;
		}

		#region Простые методы

		/// <summary>
		/// Нажать на ссылку 'еще'
		/// </summary>
		public YandexMainPage ClickMoreLink()
		{
			CustomTestContext.WriteLine("Нажать на ссылку 'еще'");
			MoreLink.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на ссылку 'Расписание'
		/// </summary>
		public ScheduleMainPage ClickScheduleLink()
		{
			CustomTestContext.WriteLine("Нажать на ссылку 'Расписание'");
			ScheduleLink.Click();

			return new ScheduleMainPage(Driver).LoadPage();
		}

		#endregion

		#region Составные методы

		public ScheduleMainPage OpenScheduleMainPage()
		{
			ClickMoreLink();
			var scheduleMainPage = ClickScheduleLink();

			return scheduleMainPage;
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		public bool IsYandexMainPageOpened()
		{
			return WaitUntilElementIsDisplay(By.XPath(MORE_LINK));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = MORE_LINK)]
		protected IWebElement MoreLink { get; set; }

		[FindsBy(How = How.XPath, Using = SCHEDULE_LINK)]
		protected IWebElement ScheduleLink { get; set; }

		#endregion

		#region Описание XPath элементов

		private const string PAGE_URL = "https://yandex.ru/";
		private const string TITLE = "//title[text()='Яндекс']";

		private const string NAVIGATION_LINKS_PANEL = "//div[@role='navigation']";
		private const string MORE_LINK = NAVIGATION_LINKS_PANEL + "//a[@data-statlog='tabs.more']";

		private const string SCHEDULE_LINK = "//a[@data-statlog='tabsPopup.rasp']";

		#endregion
	}
}
