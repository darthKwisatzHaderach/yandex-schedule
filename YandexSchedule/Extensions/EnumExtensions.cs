using System;
using System.ComponentModel;

namespace YandexSchedule.Extensions
{
	public static class EnumExtensions
	{
		public static string Description(this Enum value)
		{
			var descriptionAttribute = (DescriptionAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
			return descriptionAttribute.Length > 0 ? descriptionAttribute[0].Description : value.ToString();
		}

		public static T GetValueFromDescription<T>(string description)
		{
			var type = typeof(T);

			if (!type.IsEnum)
			{
				throw new Exception(String.Format("Тип {0} не является перечислением.", type));
			}

			foreach (var field in type.GetFields())
			{
				var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
				if (attribute != null)
				{
					if (attribute.Description == description)
					{
						return (T)field.GetValue(null);
					}
				}
				else
				{
					if (field.Name == description)
					{
						return (T)field.GetValue(null);
					}
				}
			}

			throw new Exception(String.Format("Не удалось найти значение перечисления {0} по значению атрибута Description:{1}", type, description));
		}
	}
}
