using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace DotNetBenchmarking.Regex;

/// <summary>
/// Compares different methods of building and executing a regular expression available on .net472 and .net7.
/// </summary>
[SimpleJob(RuntimeMoniker.Net472)]
[SimpleJob(RuntimeMoniker.Net70)]
[MemoryDiagnoser]
public partial class Regex
{
    [Params(10, 100, 1_000)]
    public int Loops { get; set; }

    /// <summary>
    /// Parses the input string by scanning character by character.
    /// </summary>
    [Benchmark(Baseline = true)]
    public void Manual()
    {
        for (int i = 0; i < Loops; i++)
        {
            Manual("2002-8 [3]");
        }
    }

    internal static (int year, int month, int? quarter) Manual(string input)
    {
        bool inYear = true, inMonth = false, inQuarter = false;
        int year = 0, month = 0;
        int? quarter = null;

        int i = 0;
        int value = 0;

        bool invalidInput = false;

        while ((i < input.Length) && !invalidInput)
        {
            char character = input[i];
            if (char.IsDigit(character))
            {
                value = (value * 10) + (character - '0');
            }
            else
            {
                switch (character)
                {
                    case '-':
                        if (inYear)
                        {
                            year = value;
                            value = 0;
                            inYear = false;
                            inMonth = true;
                        }
                        else
                        {
                            invalidInput = true;
                        }
                        break;

                    case ' ':
                        if (inMonth)
                        {
                            if ((i + 1) < (input.Length))
                            {
                                i++;
                                character = input[i];
                                if (character == '[')
                                {
                                    month = value;
                                    value = 0;
                                    inMonth = false;
                                    inQuarter = true;
                                }
                                else
                                {
                                    invalidInput = true;
                                }
                            }
                            else
                            {
                                invalidInput = true;
                            }
                        }
                        else
                        {
                            invalidInput = true;
                        }
                        break;
                }
            }

            i++;
        }

        if (inMonth)
        {
            month = value;
        }
        else if (inQuarter)
        {
            quarter = value;
        }
        else
        {
            invalidInput = true;
        }

        if (invalidInput)
        {
            throw new ArgumentException("Invalid input", nameof(input));
        }

        return (year, month, quarter);
    }

    /// <summary>
    /// Parses the input string by using `IndexOf`.
    /// </summary>
    [Benchmark]
    public void IndexOfString()
    {
        for (int i = 0; i < Loops; i++)
        {
            IndexOfString("2002-8 [2]");
        }
    }

    internal static (int year, int month, int? quarter) IndexOfString(string input)
    {
        int hyphenPosition = input.IndexOf('-');
        if (hyphenPosition == -1)
        {
            throw new ArgumentException("Invalid input", nameof(input));
        }

        string yearPart = input[..hyphenPosition];
        if (!int.TryParse(yearPart, out int year))
        {
            throw new ArgumentException("Invalid input", nameof(input));
        }

        int month;
        int? quarter;

        int quarterPosition = input.IndexOf(" [", hyphenPosition, StringComparison.Ordinal);
        if (quarterPosition == -1)
        {
            string monthPart = input[(hyphenPosition + 1)..];
            if (!int.TryParse(monthPart, out month))
            {
                throw new ArgumentException("Invalid input", nameof(input));
            }

            quarter = null;
        }
        else
        {
            if (!input.EndsWith("]", StringComparison.Ordinal))
            {
                throw new ArgumentException("Invalid input", nameof(input));
            }

            string monthPart = input.Substring(hyphenPosition + 1, quarterPosition - hyphenPosition - 1);
            if (!int.TryParse(monthPart, out month))
            {
                throw new ArgumentException("Invalid input", nameof(input));
            }

            string quarterPart = input.Substring(quarterPosition + 2, input.Length - quarterPosition - 3);
            if (!int.TryParse(quarterPart, out int quarterValue))
            {
                throw new ArgumentException("Invalid input", nameof(input));
            }
            quarter = quarterValue;
        }

        return (year, month, quarter);
    }

    /// <summary>
    /// Parses the input string by using a non-compiled `Regex`.
    /// </summary>
    [Benchmark]
    public void RegexNotCompiledString()
    {
        for (int i = 0; i < Loops; i++)
        {
            RegexNotCompiledString("2002-8 [2]");
        }
    }

    internal static (int year, int month, int? quarter) RegexNotCompiledString(string input) => RegexString(input, _notCompiledRegex);

#if NET7_0
#pragma warning disable SYSLIB1045 // Convert to 'GeneratedRegexAttribute'.
    private static readonly System.Text.RegularExpressions.Regex _notCompiledRegex = new(@"^(?<year>\d{4})\-(?<month>\d{1,2})( \[(?<quarter>\d{1,2})\])?$");
#pragma warning restore SYSLIB1045 // Convert to 'GeneratedRegexAttribute'.
#else
    private static readonly System.Text.RegularExpressions.Regex _notCompiledRegex = new(@"^(?<year>\d{4})\-(?<month>\d{1,2})( \[(?<quarter>\d{1,2})\])?$");
#endif

    private static (int year, int month, int? quarter) RegexString(string input, System.Text.RegularExpressions.Regex regex)
    {
        Match match = regex.Match(input);
        if (!match.Success)
        {
            throw new ArgumentException("Invalid input", nameof(input));
        }

        string yearAsString = match.Groups["year"].Value;
        int year = int.Parse(yearAsString);

        string monthAsString = match.Groups["month"].Value;
        int month = int.Parse(monthAsString);

        int? quarter;
        string quarterAsString = match.Groups["quarter"].Value;
        if (int.TryParse(quarterAsString, out int quarterValue))
        {
            quarter = quarterValue;
        }
        else
        {
            quarter = null;
        }

        return (year, month, quarter);
    }

    /// <summary>
    /// Parses the input string by using a compiled `Regex` and extracts the groups as strings.
    /// </summary>
    [Benchmark]
    public void RegexCompiledString()
    {
        for (int i = 0; i < Loops; i++)
        {
            RegexCompiledString("2002-8 [2]");
        }
    }

    internal static (int year, int month, int? quarter) RegexCompiledString(string input) => RegexString(input, _compiledRegex);

#if NET7_0
#pragma warning disable SYSLIB1045 // Convert to 'GeneratedRegexAttribute'.
    private static readonly System.Text.RegularExpressions.Regex _compiledRegex = new(@"^(?<year>\d{4})\-(?<month>\d{1,2})( \[(?<quarter>\d{1,2})\])?$", RegexOptions.Compiled);
#pragma warning restore SYSLIB1045 // Convert to 'GeneratedRegexAttribute'.
#else
    private static readonly System.Text.RegularExpressions.Regex _compiledRegex = new(@"^(?<year>\d{4})\-(?<month>\d{1,2})( \[(?<quarter>\d{1,2})\])?$", RegexOptions.Compiled);
#endif
}