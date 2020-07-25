using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BerlinClock
{
	public class TimeConverter : ITimeConverter
	{
		private const char OFF_LAMP = 'O';
		private const char YELLOW_LAMP = 'Y';
		private const char RED_LAMP = 'R';

		private const int MAX_HOUR_ROW_LAMPS = 4;
		private const int MAX_FIRST_MINUTES_ROW_LAMPS = 11;
		private const int MAX_SECOND_MINUTES_ROW_LAMPS = 4;

		public string convertTime(string aTime)
		{
			if (string.IsNullOrWhiteSpace(aTime))
			{
				throw new ArgumentNullException(nameof(aTime));
			}

			var t = aTime.Split(':');

			if (t.Length != 3)
			{
				throw new Exception($"{nameof(aTime)} has invalid format");
			}

			var hours = int.Parse(t[0]);
			var minutes = int.Parse(t[1]);
			var seconds = int.Parse(t[2]);

			var topLamp = seconds % 2 > 0 ? OFF_LAMP : YELLOW_LAMP;
			var firstHourRow = GenerateLamps(RED_LAMP, hours / 5);
			var secondHourRow = GenerateLamps(RED_LAMP, hours % 5);

			var firstMinuteRow = GenerateLamps(YELLOW_LAMP, minutes / 5);
			var secondMinuteRow = GenerateLamps(YELLOW_LAMP,
				minutes % (5 * (firstMinuteRow.Any() ? firstMinuteRow.Count() : 1)));

			var result = new StringBuilder();

			result.AppendLine(topLamp.ToString());

			result.AppendLine(
				AddOffLamps(firstHourRow, MAX_HOUR_ROW_LAMPS)
			);

			result.AppendLine(
				AddOffLamps(secondHourRow, MAX_HOUR_ROW_LAMPS)
			);

			result.AppendLine(
				AddQuarters(
					AddOffLamps(firstMinuteRow, MAX_FIRST_MINUTES_ROW_LAMPS)
				)
			);

			result.Append(
				AddOffLamps(secondMinuteRow, MAX_SECOND_MINUTES_ROW_LAMPS)
			);

			return result.ToString();
		}

		private static IEnumerable<char> GetLamps(char lampColor)
		{
			while (true)
			{
				yield return lampColor;
			}
		}

		private static string GenerateLamps(char lampColor, int count)
		{
			var result = new StringBuilder();
			foreach (var c in GetLamps(lampColor).Take(count))
			{
				result.Append(c);
			}

			return result.ToString();
		}

		private static string AddOffLamps(string lamps, int maxCount)
		{
			var lampsCount = lamps.Count();
			if (lampsCount >= maxCount)
			{
				return lamps;
			}

			var offs = GenerateLamps(OFF_LAMP, maxCount - lampsCount);
			return $"{lamps}{offs}";
		}

		private static string AddQuarters(string lamps)
		{
			var s = new StringBuilder(lamps);

			if (s[2] == YELLOW_LAMP)
			{
				s[2] = RED_LAMP;
			}
			if (s[5] == YELLOW_LAMP)
			{
				s[5] = RED_LAMP;
			}
			if (s[8] == YELLOW_LAMP)
			{
				s[8] = RED_LAMP;
			}

			return s.ToString();
		}
	}
}
