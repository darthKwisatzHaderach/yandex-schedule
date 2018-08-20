using System;
using System.Text.RegularExpressions;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using YandexSchedule.DataStructures;
using YandexSchedule.Extensions;

namespace YandexSchedule.Pages
{
	public abstract class BasePage
	{
		public BasePage(IWebDriver driver)
		{
			Driver = driver;
			PageFactory.InitElements(driver, this);
		}

		public IWebDriver Driver { get; set; }

		/// <summary>
		/// Получить текст из элемента
		/// </summary>
		/// <param name="by">локатор</param>
		public string GetElementText(By by)
		{
			try
			{
				return Driver.FindElement(by).Text;
			}
			catch (NoSuchElementException)
			{
				CustomTestContext.WriteLine("Элемент не найден");
				return null;
			}
		}

		/// <summary>
		/// Получить число из элемента
		/// </summary>
		/// <param name="by">локатор</param>
		public int GetNumberFromElement(By by)
		{
			try
			{
				var text = Driver.FindElement(by).Text;
				int result;
				int.TryParse(Regex.Replace(text, "[^.0-9]", ""), out result);
				return result;
			}
			catch (NoSuchElementException)
			{
				return 0;
			}
		}

		/// <summary>
		/// Получить дату и время из элемента
		/// </summary>
		/// <param name="by">локатор</param>
		public DateTime GetDateTimeFromElement(By by)
		{
			try
			{
				var dateTime = Driver.FindElement(by).Text;
				DateTime time;
				DateTime.TryParse(dateTime, out time);
				return time;
			}
			catch (NoSuchElementException)
			{
				return default(DateTime);
			}
		}

		/// <summary>
		/// Получить временной диапазон из элемента
		/// </summary>
		/// <param name="by">локатор</param>
		public TimeSpan GetTimeFromElement(By by, string[] formats = null)
		{
			try
			{
				var dateTime = Driver.FindElement(by).Text;
				TimeSpan time;

				if (formats != null)
				{
					return TimeSpan.ParseExact(dateTime, formats, System.Globalization.CultureInfo.InvariantCulture);
				}

				TimeSpan.TryParse(dateTime, out time);
				return time;
			}
			catch (NoSuchElementException)
			{
				return default(TimeSpan);
			}
		}

		/// <summary>
		/// Получить валюту из элемента
		/// </summary>
		/// <param name="by">локатор</param>
		public Currency GetCurrencyFromElement(By by)
		{
			var currency = Driver.FindElement(by).Text;

			if (currency == Currency.RUB.Description())
			{
				return Currency.RUB;
			}

			if (currency == Currency.USD.Description())
			{
				return Currency.USD;
			}

			return Currency.None;
		}

		/// <summary>
		/// Ожидание появления элемента
		/// </summary>
		/// <param name="by">локатор</param>
		/// <param name="timeout">время ожидания</param>
		public bool WaitUntilElementIsDisplay(By by, int timeout = 10)
		{
			var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeout));

			try
			{
				return wait.Until(d => Driver.FindElement(by).Displayed);
			}
			catch (WebDriverTimeoutException)
			{
				return false;
			}
			catch (StaleElementReferenceException)
			{
				return wait.Until(d => Driver.FindElement(by).Displayed);
			}
		}
	}
}
