using System;

using YandexSchedule.DataStructures;

namespace YandexSchedule.Models
{
	public class Trip
	{
		public string Number { get; set; }
		public string PointFrom { get; set; }
		public string PointTo { get; set; }
		public DateTime StartDateTime { get; set; }
		public TimeSpan Duration { get; set; }
		public DateTime EndDateTime { get; set; }
		public int Price { get; set; }
		public Currency Currency { get; set; }

		public Trip(
			string number = null,
			string pointFrom = null,
			string pointTo = null,
			DateTime startDateTime = default(DateTime),
			TimeSpan duration = default(TimeSpan),
			DateTime endDateTime = default(DateTime),
			int price = 0,
			Currency currency = Currency.None)
		{
			Number = number;
			PointFrom = pointFrom;
			PointTo = pointTo;
			StartDateTime = startDateTime;
			Duration = duration;
			EndDateTime = endDateTime;
			Price = price;
			Currency = currency;
		}

	}
}
