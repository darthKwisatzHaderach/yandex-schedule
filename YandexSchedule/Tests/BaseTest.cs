using System;
using System.IO;

using NUnit.Framework;
using NUnit.Framework.Interfaces;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace YandexSchedule.Tests
{
	public class BaseTest
	{
		public IWebDriver Driver;

		[OneTimeSetUp]
		public void SetUp()
		{
			Driver = new ChromeDriver();
			Driver.Manage().Window.Maximize();
		}

		[TearDown]
		public void TearDown()
		{
			if (TestContext.CurrentContext.Result.Outcome.Status.Equals(TestStatus.Failed))
			{
				TakeScreenshot(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestResults"));
			}
		}

		[OneTimeTearDown]
		public void OneTimeTearDown()
		{
			Driver.Quit();
			Driver.Dispose();
		}

		public void TakeScreenshot(string path)
		{
			Directory.CreateDirectory(path);

			string screenName = TestContext.CurrentContext.Test.Name;
			string fileName = $"{Path.Combine(path, screenName)}.png";

			var screenShot = ((ITakesScreenshot)Driver).GetScreenshot();

			screenShot.SaveAsFile(fileName, ScreenshotImageFormat.Png);
		}

	}
}
