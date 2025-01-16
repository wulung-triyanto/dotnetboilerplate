using Common.Core.Enum;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Utf8Json.Resolvers;

namespace Common.Core.Extension;

public static class AppExtension
{
    #region DATE TIME RELATED
    public static string GetDayName(this int scheduleDayId)
    {
        return (DayOfWeek)scheduleDayId switch
        {
            DayOfWeek.Monday => DayOfWeek.Monday.ToString(),
            DayOfWeek.Tuesday => DayOfWeek.Tuesday.ToString(),
            DayOfWeek.Wednesday => DayOfWeek.Wednesday.ToString(),
            DayOfWeek.Thursday => DayOfWeek.Thursday.ToString(),
            DayOfWeek.Friday => DayOfWeek.Friday.ToString(),
            DayOfWeek.Saturday => DayOfWeek.Saturday.ToString(),
            DayOfWeek.Sunday => DayOfWeek.Sunday.ToString(),
            _ => string.Empty,
        };
    }

    public static DateTime FirstDayOfMonth(this DateTime value)
    {
        return new DateTime(value.Year, value.Month, 1);
    }

    public static bool IsDateEven(this DateTime dateInput)
    {
        var PlateNumber = dateInput.Date.Day;
        var IsEven = PlateNumber % 2 == 0;
        return IsEven;
    }

    public static DateTime LastDayOfMonth(this DateTime value)
    {
        return value.FirstDayOfMonth().AddMonths(1).AddDays(-1);
    }

    public static IEnumerable<DateTime> Range(this DateTime startDate, DateTime endDate)
    {
        return Enumerable.Range(0, (endDate - startDate).Days + 1).Select(d => startDate.AddDays(d));
    }

    public static string TzOffsetToTzName(this int tzOffset)
    {
        return tzOffset switch
        {
            7 => "WIB (GMT+7)",
            8 => "WITA (GMT+8)",
            9 => "WIT (GMT+9)",
            _ => "Unknown Timezone"
        };
    }
    #endregion

    #region LINQ RELATED EXTENSIONS
    public static IQueryable<TEntity> ApplyFilters<TEntity, TFilter>(this IQueryable<TEntity> query, TFilter filter)
    {
        filter?.GetType()
               .GetProperties()
               .Where(info => !(info.Name is "page" or "row" or "orderBy" or "sortBy" || info.Name.EndsWith("Id")))
               .ToList()
               .ForEach(info =>
               {
                   query = query.WhereIf(info.GetValue(filter)?.ToString() != null, x => x!
                                             .GetType()
                                             .GetProperty(info.Name)!
                                             .GetValue(x)!
                                             .ToString()!
                                             .Contains(info.GetValue(filter)!.ToString()!)
                   );
               });

        return query;
    }

    public static IEnumerable<TSource> Pagination<TSource>(this IEnumerable<TSource> source, int page, int row)
    {
        return source.Skip((page - 1) * row).Take(row);
    }

    public static IQueryable<TSource> Pagination<TSource>(this IQueryable<TSource> source, int page, int row)
    {
        return source.Skip((page - 1) * row).Take(row);
    }

    public static IEnumerable<TSource> SortBy<TSource, TKey>(this IEnumerable<TSource> source, bool condition, OrderType order, Func<TSource, TKey> keySelector)
    {
        if (!condition) return source;

        return order == OrderType.ASC ? source.OrderBy(keySelector) : source.OrderByDescending(keySelector);
    }

    public static IQueryable<TSource> SortBy<TSource, TKey>(this IQueryable<TSource> source, bool condition, OrderType order, Expression<Func<TSource, TKey>> keySelector)
    {
        if (!condition) return source;

        return order == OrderType.ASC ? source.OrderBy(keySelector) : source.OrderByDescending(keySelector);
    }

    public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> query, bool condition, Func<T, bool> whereClause)
    {
        return condition ? query.Where(whereClause) : query;
    }

    public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> query, bool condition, Func<T, bool> whereClause, Func<T, bool> elseIfClause)
    {
        return condition ? query.Where(whereClause) : query.Where(elseIfClause);
    }

    /// <summary>
    /// If condition is True, then apply where clause to query.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="condition"></param>
    /// <param name="whereClause"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> whereClause)
    {
        return condition ? query.Where(whereClause) : query;
    }

    /// <summary>
    /// If condition is True, then apply where clause to query.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="valueForCheck"></param>
    /// <param name="whereClause"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, string valueForCheck, Expression<Func<T, bool>> whereClause)
    {
        return !string.IsNullOrEmpty(valueForCheck) ? query.Where(whereClause) : query;
    }
    #endregion

    #region SERIALIZER
    /// <summary>
    /// Deserialize string into class object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <returns></returns>
    public static T? Deserialize<T>(this string json, JSONEngineType engine = JSONEngineType.DEFAULT)
    {
        var result = default(T);

        switch (engine)
        {
            case JSONEngineType.DEFAULT:
                result = JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = false,
                    NumberHandling = JsonNumberHandling.AllowReadingFromString,
                    Converters = { new JsonStringEnumConverter() }
                });
                break;
            case JSONEngineType.UTF8JSON:
                result = Utf8Json.JsonSerializer.Deserialize<T>(json, StandardResolver.CamelCase);
                break;
            default:
                break;
        }

        return result;
    }

    /// <summary>
    /// Serialize class object into JSON string
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="model"></param>
    /// <param name="isPretty"></param>
    /// <returns></returns>
    public static string Serialize<T>(this T model, bool isPretty = false, JSONEngineType engine = JSONEngineType.DEFAULT)
    {
        var result = string.Empty;

        switch (engine)
        {
            case JSONEngineType.DEFAULT:
                result = JsonSerializer.Serialize(model, new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = isPretty,
                });
                break;
            case JSONEngineType.UTF8JSON:
                if (isPretty)
                {
                    var json = Utf8Json.JsonSerializer.Serialize(model, StandardResolver.CamelCase);
                    result = Utf8Json.JsonSerializer.PrettyPrint(json);

                    return result;
                }

                result = Utf8Json.JsonSerializer.ToJsonString(model, StandardResolver.CamelCase);
                break;
            default:
                break;
        }

        return result;
    }
    #endregion

    public static string AddSemicolonToSentence(this string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return string.Empty;
        }

        StringBuilder newText = new(text.Length * 2);

        newText.Append(text[0]);

        for (int i = 1; i < text.Length; i++)
        {
            if (char.IsUpper(text[i]))
                if (text[i - 1] != ' ' && !char.IsUpper(text[i - 1]) ||
                    char.IsUpper(text[i - 1]) &&
                     i < text.Length - 1 && !char.IsUpper(text[i + 1]))
                    newText.Append(';');
            newText.Append(text[i]);
        }

        return newText.ToString();
    }

    public static string AddSpacesToSentence(this string text, bool preserveAcronyms)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return string.Empty;
        }

        StringBuilder newText = new(text.Length * 2);

        newText.Append(text[0]);

        for (int i = 1; i < text.Length; i++)
        {
            if (char.IsUpper(text[i]))
                if (text[i - 1] != ' ' && !char.IsUpper(text[i - 1]) ||
                    preserveAcronyms && char.IsUpper(text[i - 1]) &&
                     i < text.Length - 1 && !char.IsUpper(text[i + 1]))
                    newText.Append(' ');
            newText.Append(text[i]);
        }

        return newText.ToString();
    }

    public static decimal GetDistance(this decimal requestLatitude, decimal requestLongitude, decimal latitude, decimal longitude)
    {
        var distance = 6371 * Math.Acos(Math.Cos(requestLatitude.GetRadians()) * Math.Cos(latitude.GetRadians()) *
                       Math.Cos(longitude.GetRadians() - requestLongitude.GetRadians()) + Math.Sin(requestLatitude.GetRadians()) *
                       Math.Sin(latitude.GetRadians()));
        var distanceDecimal = Convert.ToDecimal(distance);

        return distanceDecimal;
    }

    public static double GetRadians(this decimal degree)
    {
        var degreeDouble = Convert.ToDouble(degree);
        var radians = Math.PI / 180 * degreeDouble;

        return radians;
    }

    /// <summary>
    /// Check if the IEnumerable is null or contains no elements.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static bool IsEmpty<TSource>(this IEnumerable<TSource>? source)
    {
        if (source == null || !source.Any()) { return true; }
        return false;
    }

    /// <summary>
    /// Check if string is null, string empty or only contain whitespaces
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static bool IsEmpty(this string source)
    {
        if (string.IsNullOrWhiteSpace(source)) { return true; }
        return false;
    }

    /// <summary>
    /// Compare two string using OrdinalIgnoreCase
    /// </summary>
    /// <param name="text"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static bool IsEqual(this string? text, string comparer)
    {
        if (text == null) return false;
        if (text.Equals(comparer, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return false;
    }

    public static bool IsLicensePlateEven(this string licensePlate)
    {
        var regex = new Regex(@"[0-9]+");
        var match = regex.Match(licensePlate);
        var PlateNumber = int.Parse(match.Value);
        var IsEven = PlateNumber % 2 == 0;
        return IsEven;
    }

    /// <summary>
    /// Check whether the object is of a numeric type or not.
    /// </summary>
    /// <param name="o"></param>
    /// <returns></returns>
    public static bool IsNumericType(this object o)
    {
        switch (Type.GetTypeCode(o.GetType()))
        {
            case TypeCode.Byte:
            case TypeCode.SByte:
            case TypeCode.UInt16:
            case TypeCode.UInt32:
            case TypeCode.UInt64:
            case TypeCode.Int16:
            case TypeCode.Int32:
            case TypeCode.Int64:
            case TypeCode.Decimal:
            case TypeCode.Double:
            case TypeCode.Single:
                return true;
            default:
                return false;
        }
    }

    /// <summary>
    /// Check if IEnumerable is not null or contains any elements.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static bool NotEmpty<TSource>(this IEnumerable<TSource>? source)
    {
        return !source.IsEmpty();
    }

    /// <summary>
    /// Returns a random item from a given collection.
    /// </summary>
    /// <typeparam name="T">The type of the items in the collection.</typeparam>
    /// <param name="collection">The collection from which to pick a random item.</param>
    /// <returns>A randomly selected item from the collection.</returns>
    public static T RandomPick<T>(this List<T> collection)
    {
        var random = new Random();
        var randomItem = collection.ElementAt(random.Next(0, collection.Count));

        return randomItem;
    }

    /// <summary>
    /// Remove line feed and carriage return from string
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string RemoveLFCR(this string text)
    {
        RegexOptions options = RegexOptions.None;
        Regex regex = new("[ ]{2,}", options);

        string result = text.Replace(@"\\", string.Empty);
        result = text.Replace(Environment.NewLine, string.Empty);
        result = regex.Replace(result, " ");

        return result;
    }

    /// <summary>
    /// Convert the string value into camel case
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string ToCamelCase(this string input)
    {
        if (string.IsNullOrEmpty(input) || char.IsLower(input[0]))
        {
            return input;
        }

        var chars = input.ToCharArray();
        chars[0] = char.ToLowerInvariant(input[0]);

        return new(chars);
    }

    /// <summary>
    /// Convert integer value into a certain enumeration
    /// </summary>
    /// <typeparam name="TEnum">Enumeration</typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static TEnum? ToEnum<TEnum>(this int? value) where TEnum : System.Enum
    {
        if (value == null) { return default; }
        return (TEnum)System.Enum.ToObject(typeof(TEnum), value);
    }    

    public static string? ToEnumString<TEnum>(this int value) where TEnum : System.Enum
    {
        return System.Enum.IsDefined(typeof(TEnum), value) ? System.Enum.GetName(typeof(TEnum), value) : value.ToString();
    }

    public static string? ToEnumString<TEnum>(this int? value) where TEnum : System.Enum
    {
        return System.Enum.IsDefined(typeof(TEnum), value ?? 0) ? System.Enum.GetName(typeof(TEnum), value ?? 0) : value.ToString();
    }

    /// <summary>
    /// Convert general [ResponseStatus] into more specific [GRPCStatus].
    /// If [ResponseStatus].[SUCCESS] return [GRPCStatus].[OK], otherwise [GRPCStatus].[NOT_FOUND]
    /// </summary>
    /// <param name="status">The [ResponseStatus] value</param>
    /// <param name="negativeStatus">[Optional] Custom negative [GRPCStatus] to be used when the [ResponseStatus] is not [SUCCESS]</param>
    /// <returns></returns>
    public static GRPCStatus ToGRPCStatus(this ResponseStatus status,
                                          GRPCStatus? negativeStatus = GRPCStatus.NOT_FOUND)
    {
        if (negativeStatus.HasValue)
        {
            return status == ResponseStatus.SUCCESS ? GRPCStatus.OK : negativeStatus.Value;
        }
        return status == ResponseStatus.SUCCESS ? GRPCStatus.OK : GRPCStatus.NOT_FOUND;
    }

    /// <summary>
    /// Convert value-separated string to List of T.
    /// <example>
    /// Example: "1,2,3,4,5" will be converted to List { 1, 2, 3, 4, 5 } if separator is comma (,)
    /// </example>
    /// </summary>
    /// <param name="value">a value-separated string</param><br></br>
    /// <param name="separator">value separator like a single comma or semicolon</param>
    /// <typeparam name="T">generic type</typeparam>
    /// <returns>List of T</returns>
    public static List<T> ToList<T>(this string? value, char[] separator)
    {
        return value?.Split(separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries) // ignore empty and spaces
                     .Select(x =>
                     {
                         try
                         {
                             return Convert.ChangeType(x, typeof(T));
                         }
                         catch (Exception e)
                         {
                             return default;
                         }
                     }).OfType<T>().ToList() ?? [];
    }

    /// <summary int="{ 1, 2, 3, 4, 5 }">
    /// Convert value-separated string to List of T, where value separator is comma (,).
    /// <example>
    /// Example: "1,2,3,4,5" will be converted to List { 1, 2, 3, 4, 5 }
    /// </example>
    /// </summary>
    /// <param name="value">a value-separated string</param>
    /// <typeparam name="T">generic type</typeparam>
    /// <returns>List of T</returns>
    public static List<T> ToList<T>(this string? value)
    {
        // split string by comma
        return value?.ToList<T>([',']) ?? [];
    }

    public static async Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> items, CancellationToken cancellationToken = default)
    {
        var results = new List<T>();
        await foreach (var item in items.WithCancellation(cancellationToken).ConfigureAwait(false))
            results.Add(item);

        return results;
    }

    public static string ToTitleCase(this string input)
    {
        return Regex.Replace(input,
            @"(^|[a-z])([A-Z])",
            m => m.Groups[1].Value + " " + m.Groups[2].Value
        ).Trim();
    }

    /// <summary>
    /// Convert boolean value into string
    /// </summary>
    /// <param name="source"></param>
    /// <returns><b>"Yes"</b> if the [source] is <b>true</b>, <b>"No"</b> if the [source] is <b>false</b> and <b>"-"</b> if the [source] is <b>null</b></returns>
    public static string TrueToYes(this bool? source)
    {
        if (!source.HasValue) { return "-"; }
        return source.Value ? "Yes" : "No";
    }

    /// <summary>
    /// Await multiple list of Tasks
    /// </summary>
    /// <param name="tasks"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    public static async Task<TResult[]> WhenAll<TResult>(this IEnumerable<Task<TResult>> tasks)
    {
        return await Task.WhenAll(tasks);
    }
}