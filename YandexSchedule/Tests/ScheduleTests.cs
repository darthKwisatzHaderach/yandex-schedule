using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using YandexSchedule.DataStructures;
using YandexSchedule.Extensions;
using YandexSchedule.Models;
using YandexSchedule.Pages;

namespace YandexSchedule.Tests
{
	public class ScheduleTests : BaseTest
	{
		[SetUp]
		public void ScheduleTestsSetUp()
		{
			_yandexMainPage = new YandexMainPage(Driver);
			_scheduleMainPage = new ScheduleMainPage(Driver);
			_trainDetailsPage = new TrainDetailsPage(Driver);
		}

		[Test]
		public void ScheduleTest()
		{
			string pointFrom = City.Yekaterinburg.Description();
			string pointTo = City.KamenskUralsky.Description();
			DateTime date = DateTime.Today.Next(DayOfWeek.Saturday);
			DateTime expectedTime = date + TimeSpan.FromHours(12);
			int expectedPrice = 200;

			_yandexMainPage
				.GetPage()
				.OpenScheduleMainPage();

			_scheduleMainPage
				.ClickSuburbanTypeButton()
				.Search(pointFrom: pointFrom, pointTo: pointTo, date: date);

			List<Trip> trips = _scheduleMainPage.ParseSearchResultsTable();

			Trip expectedTrip = trips
				.Where(x => x.StartDateTime < expectedTime)
				.Where(x => x.Price < expectedPrice)
				.OrderBy(x => x.StartDateTime)
				.ThenByDescending(x => x.Price)
				.First();

			CustomTestContext.WriteLine($"{expectedTrip.Number} | {expectedTrip.PointFrom} - " +
					$"{expectedTrip.PointTo} | {expectedTrip.StartDateTime} - {expectedTrip.EndDateTime} | " +
					$"{expectedTrip.Duration} | " + $"{expectedTrip.Price} {expectedTrip.Currency}");

			_scheduleMainPage.OpenTrainDetails(expectedTrip.Number);

			Assert.IsTrue(_trainDetailsPage.IsHeaderContainsText(expectedTrip.Number));
			Assert.AreEqual(expectedTrip.PointFrom, _trainDetailsPage.GetPointFromText());
			Assert.AreEqual(pointTo, _trainDetailsPage.GetPointToText());
			Assert.AreEqual(expectedTrip.Duration, _trainDetailsPage.GetTripDuration());
		}

		private YandexMainPage _yandexMainPage;
		private ScheduleMainPage _scheduleMainPage;
		private TrainDetailsPage _trainDetailsPage;
	}
}
