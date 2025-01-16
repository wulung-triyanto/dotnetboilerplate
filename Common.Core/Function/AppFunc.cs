using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Common.Core.Function;

public static class AppFunc
{
    public static int CalculateTimeGap(DateTime? targetTime, DateTime? actualTime)
    {
        return targetTime.HasValue && actualTime.HasValue ? (int)(actualTime.Value - targetTime.Value).TotalMinutes : 0;
    }

    public static int DaysInMonth(int year, int month)
    {
        // Validate month input
        if (month < 1 || month > 12)
        {
            throw new ArgumentOutOfRangeException(nameof(month), "Month must be between 1 and 12.");
        }

        // Handle the number of days in each month
        switch (month)
        {
            case 1: // January
            case 3: // March
            case 5: // May
            case 7: // July
            case 8: // August
            case 10: // October
            case 12: // December
                return 31;

            case 4: // April
            case 6: // June
            case 9: // September
            case 11: // November
                return 30;

            case 2: // February
                    // Check for leap year
                if (DateTime.IsLeapYear(year))
                {
                    return 29;
                }
                return 28;

            default:
                return 0; // This should not happen due to earlier validation
        }
    }

    /// <summary>
    /// Generate a Shared Access Signature (SAS) token, which is typically required to access Azure-related resources (e.g., Blob Storage, Service Bus, etc.).
    /// </summary>
    /// <param name="resourceUri"></param>
    /// <param name="keyName"></param>
    /// <param name="key"></param>
    /// <param name="lifetime">The SAS token lifetime in second (e.g., 604800 for a week lifetime)</param>
    /// <returns></returns>
    public static string GenerateSASToken(string resourceUri, string keyName, string key, int lifetime)
    {
        TimeSpan sinceEpoch = DateTime.UtcNow - new DateTime(1970, 1, 1);
        var expiry = Convert.ToString((int)sinceEpoch.TotalSeconds + lifetime);
        string stringToSign = HttpUtility.UrlEncode(resourceUri) + "\n" + expiry;
        HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
        var signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign)));
        var sasToken = string.Format(CultureInfo.InvariantCulture, "SharedAccessSignature sr={0}&sig={1}&se={2}&skn={3}", HttpUtility.UrlEncode(resourceUri), HttpUtility.UrlEncode(signature), expiry, keyName);
        return sasToken;
    }

    public static IEnumerable<DateTime> GetAvailableDates(DateTime startDate, DateTime endDate, List<(DateTime, DateTime)> movements)
    {
        var availableDates = new List<DateTime>();
        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            availableDates.Add(date);
        }

        foreach (var (movementStartDate, movementEndDate) in movements)
        {
            for (var date = movementStartDate; date <= movementEndDate; date = date.AddDays(1))
            {
                availableDates.Remove(date);
            }
        }

        return availableDates;
    }

    public static string? GetEnumValues<TEnum>(string lastDelim = "or") where TEnum : struct, System.Enum
    {
        var list = System.Enum.GetValues<TEnum>().Select(x => x).Cast<int>().Select(x => x.ToString()).ToList();
        return list.Count > 1 ? string.Join(", ", list.Take(list.Count - 1)) + $" {lastDelim} " + list.Last() : list.FirstOrDefault();
    }

    public static string GetEnumValueString<TEnum>(string lastDelim = "or") where TEnum : struct, System.Enum
    {
        return string.Join(",", GetEnumValues<TEnum>(lastDelim));
    }

    public static List<(int, int)> GetTotalDays(DateTime startDate, DateTime endDate)
    {
        List<(int, int)> monthAndDay = []; //Month and day
        var endDateMonth = endDate.Month;
        var endDateDay = endDate.Day;
        var endDateHour = endDate.Hour;
        var startDateMonth = startDate.Month;
        var startDateDay = startDate.Day;
        var startDateYear = startDate.Year;
        var startDateHour = startDate.Hour;

        endDateMonth = startDateMonth + endDateMonth;
        for (int month = startDateMonth; month <= endDateMonth; month++)
        {
            int currentMonth = month > 12 ? month - 12 : month;
            int daysInMonth = DateTime.DaysInMonth(startDateYear, currentMonth);

            int totalDays = daysInMonth * 24;

            if (month == startDateMonth)
            {
                totalDays = (DaysInMonth(startDateYear, startDateMonth) - startDateDay) * 24 - startDateHour;
            }
            else if (month == endDateMonth)
            {
                totalDays = endDateDay * 24 + endDateHour;
            }
            monthAndDay.Add((currentMonth, totalDays));
        }

        return monthAndDay;
    }

    /// <summary>
    /// Generate composite unique string that consist of random 8 alphabet characters
    /// combined with UNIX date time format strings.
    /// </summary>
    public static string TimestampComposite()
    {
        string result = string.Empty;

        int length = 9;
        Random random = new();

        for (int i = 0; i < length; i++)
        {
            result += ((char)(random.Next(1, 26) + 64)).ToString().ToUpper();
        }

        double strTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        result += strTimestamp.ToString().ToUpper();

        return result;
    }
}
