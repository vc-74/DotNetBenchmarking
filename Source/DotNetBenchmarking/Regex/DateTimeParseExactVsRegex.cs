using System.Globalization;
using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace DotNetBenchmarking.Regex;

#if NET7_0
/// <summary>
/// Compares different methods of parsing a date/time in the ISO 8601 format.
/// </summary>
[SimpleJob(RuntimeMoniker.Net70)]
[MemoryDiagnoser]
public partial class DateTimeParseExactVsRegex
{
    [Params(10, 100, 1_000)]
    public int Loops { get; set; }

    /// <summary>
    /// Parses the input string by using <see cref="DateTime.ParseExact"/> 
    /// with a custom format.
    /// </summary>
    [Benchmark(Baseline = true)]
    public void ParseExactCustomFormat()
    {
        DateTime.ParseExact(_serializedDateTime, "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffffK",
            CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
    }

    private const string _serializedDateTime = "2002-08-11T10:11:12.0000000Z";

    /// <summary>
    /// Parses the input string by using <see cref="DateTime.ParseExact"/> 
    /// with a standard format.
    /// </summary>
    [Benchmark]
    public void ParseExactStandardFormat()
    {
        DateTime.ParseExact(_serializedDateTime, "O",
            CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
    }

    /// <summary>
    /// Parses the input string by using a <see cref="Regex"/>.
    /// </summary>
    [Benchmark]
    public void Regex()
    {
        Match match = GetRegex().Match(_serializedDateTime);
        if (!match.Success)
        {
            throw new NotImplementedException();
        }

        int GetInt(string groupName)
        {
            ReadOnlySpan<char> yearAsString = match.Groups[groupName].ValueSpan;
            return int.Parse(yearAsString);
        }

        int year = GetInt("year"), month = GetInt("month"), day = GetInt("day");
        int hour = GetInt("hour"), minute = GetInt("minute"), second = GetInt("second");
        int subSecond = GetInt("subsecond");

        DateTime _ = new DateTime(year, month, day, hour, minute, second).AddTicks(subSecond);
    }

    [GeneratedRegex(@"^(?<year>\d{4})\-(?<month>\d{2})\-(?<day>\d{2})T(?<hour>\d{2})\:(?<minute>\d{2})\:(?<second>\d{2})\.(?<subsecond>\d{7})Z$")]
    private static partial System.Text.RegularExpressions.Regex GetRegex();

    /// <summary>
    /// Detects whether the input string matches the date/time format by using a <see cref="Regex"/>.
    /// </summary>
    [Benchmark]
    public void RegexIsMatch() => GetRegex().IsMatch(_serializedDateTime);
}
#endif
