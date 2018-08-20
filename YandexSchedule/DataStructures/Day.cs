using System.ComponentModel;

namespace YandexSchedule.DataStructures
{
	public enum Day
	{
		[Description("Понедельник")]
		Monday,
		[Description("Вторник")]
		Tuesday,
		[Description("Среда")]
		Wednesday,
		[Description("Четверг")]
		Thursday,
		[Description("Пятница")]
		Friday,
		[Description("Суббота")]
		Saturday,
		[Description("Воскресенье")]
		Sunday
	}
}
