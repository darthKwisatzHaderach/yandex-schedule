﻿using System;

namespace YandexSchedule.Extensions
{
	static class DateTimeExtension
	{
		public static DateTime Next(this DateTime from, DayOfWeek dayOfWeek)
		{
			int start = (int)from.DayOfWeek;
			int target = (int)dayOfWeek;
			if (target <= start)
				target += 7;
			return from.AddDays(target - start);
		}
	}
}
