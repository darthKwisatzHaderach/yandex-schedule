

using System.ComponentModel;

namespace YandexSchedule.DataStructures
{
	public enum Currency
	{
		[Description("Р")]
		RUB,
		[Description("$")]
		USD,
		[Description("")]
		None
	}
}
