using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace DotNetBenchmarking.Regex;

/// <summary>
/// Compares different methods of building and executing a regular expression.
/// </summary>
[SimpleJob(RuntimeMoniker.Net472)]
[SimpleJob(RuntimeMoniker.Net70)]
[MemoryDiagnoser]
public partial class Regex
{
    /// <summary>
    /// Parses the input string by scanning character by character.
    /// </summary>
    [Benchmark(Baseline = true)]
    public void Manual()
    {
        const string input = "2008-8 [3]";
        (int year, int month, int? quarter) = Manual(input);

        if ((year != 2008) || (month != 8) || (quarter != 2))
        {
            throw new Exception();
        }
    }

    private static (int year, int month, int? quarter) Manual(string input)
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
        const string input = "2008-8 [2]";
        (int year, int month, int? quarter) = IndexOfString(input);

        if ((year != 2008) || (month != 8) || (quarter != 2))
        {
            throw new Exception();
        }
    }

    private static (int year, int month, int? quarter) IndexOfString(string input)
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

        int quarterPosition = input.IndexOf(" [", hyphenPosition);
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
            if (!input.EndsWith("]"))
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

#if NET7_0
    /// <summary>
    /// Parses the input string by using `IndexOf` on ranges.
    /// </summary>
    [Benchmark]
    public void IndexOfRanges()
    {
        const string input = "2008-8 [2]";
        (int year, int month, int? quarter) = IndexOfRanges(input);

        if ((year != 2008) || (month != 8) || (quarter != 2))
        {
            throw new Exception();
        }
    }

    private static (int year, int month, int? quarter) IndexOfRanges(string input)
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

        int quarterPosition = input.IndexOf(" [", hyphenPosition);
        if (quarterPosition == -1)
        {
            string monthPart = input[(hyphenPosition + 2)..];
            if (!int.TryParse(monthPart, out month))
            {
                throw new ArgumentException("Invalid input", nameof(input));
            }

            quarter = null;
        }
        else
        {
            if (!input.EndsWith("]"))
            {
                throw new ArgumentException("Invalid input", nameof(input));
            }

            string monthPart = input[(hyphenPosition + 1)..(quarterPosition + 1)];
            if (!int.TryParse(monthPart, out month))
            {
                throw new ArgumentException("Invalid input", nameof(input));
            }

            string quarterPart = input[(quarterPosition + 2)..(input.Length - 1)];
            if (!int.TryParse(quarterPart, out int quarterValue))
            {
                throw new ArgumentException("Invalid input", nameof(input));
            }
            quarter = quarterValue;
        }

        return (year, month, quarter);
    }

    /// <summary>
    /// Parses the input string by using `IndexOf` on `ReadOnlySpan&lt;char&gt;`.
    /// </summary>
    [Benchmark]
    public void IndexOfSpans()
    {
        const string input = "2008-8 [2]";
        (int year, int month, int? quarter) = IndexOfSpans(input);

        if ((year != 2008) || (month != 8) || (quarter != 2))
        {
            throw new Exception();
        }
    }

    private static (int year, int month, int? quarter) IndexOfSpans(string input)
    {
        ReadOnlySpan<char> currentSpan = input;

        int hyphenPosition = currentSpan.IndexOf('-');
        if (hyphenPosition == -1)
        {
            throw new ArgumentException("Invalid input", nameof(input));
        }

        ReadOnlySpan<char> yearPart = currentSpan[..hyphenPosition];
        if (!int.TryParse(yearPart, out int year))
        {
            throw new ArgumentException("Invalid input", nameof(input));
        }

        currentSpan = currentSpan[(hyphenPosition + 1)..];

        int month;
        int? quarter;

        int quarterPosition = currentSpan.IndexOf(" [");
        if (quarterPosition == -1)
        {
            if (!int.TryParse(currentSpan, out month))
            {
                throw new ArgumentException("Invalid input", nameof(input));
            }

            quarter = null;
        }
        else
        {
            if (!currentSpan.EndsWith("]"))
            {
                throw new ArgumentException("Invalid input", nameof(input));
            }

            ReadOnlySpan<char> monthPart = currentSpan[0..quarterPosition];
            if (!int.TryParse(monthPart, out month))
            {
                throw new ArgumentException("Invalid input", nameof(input));
            }

            ReadOnlySpan<char> quarterPart = currentSpan[(quarterPosition + 2)..^1];
            if (!int.TryParse(quarterPart, out int quarterValue))
            {
                throw new ArgumentException("Invalid input", nameof(input));
            }
            quarter = quarterValue;
        }

        return (year, month, quarter);
    }
#endif

    /// <summary>
    /// Parses the input string by using a non-compiled `Regex`.
    /// </summary>
    [Benchmark]
    public (int year, int month, int? quarter) RegexNotCompiled()
    {
        const string input = "2008-8 [2]";
        (int year, int month, int? quarter) = OldRegex(input, _notCompiledRegex);

        if ((year != 2008) || (month != 8) || (quarter != 2))
        {
            throw new Exception();
        }

        return (year, month, quarter);
    }
#if NET7_0
#pragma warning disable SYSLIB1045 // Convert to 'GeneratedRegexAttribute'.
    private static readonly System.Text.RegularExpressions.Regex _notCompiledRegex = new(@"^(?<year>\d{4})\-(?<month>\d{1,2})( \[(?<quarter>\d{1,2})\])?$");
#pragma warning restore SYSLIB1045 // Convert to 'GeneratedRegexAttribute'.
#else
    private static readonly System.Text.RegularExpressions.Regex _notCompiledRegex = new(@"^(?<year>\d{4})\-(?<month>\d{1,2})( \[(?<quarter>\d{1,2})\])?$");
#endif

    private static (int year, int month, int? quarter) OldRegex(string input, System.Text.RegularExpressions.Regex regex)
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
    /// Parses the input string by using a compiled `Regex`.
    /// </summary>
    [Benchmark]
    public void RegexCompiled()
    {
        const string input = "2008-8 [2]";
        (int year, int month, int? quarter) = OldRegex(input, _compiledRegex);

        if ((year != 2008) || (month != 8) || (quarter != 2))
        {
            throw new Exception();
        }
    }
#if NET7_0
#pragma warning disable SYSLIB1045 // Convert to 'GeneratedRegexAttribute'.
    private static readonly System.Text.RegularExpressions.Regex _compiledRegex = new(@"^(?<year>\d{4})\-(?<month>\d{1,2})( \[(?<quarter>\d{1,2})\])?$", RegexOptions.Compiled);
#pragma warning restore SYSLIB1045 // Convert to 'GeneratedRegexAttribute'.
#else
    private static readonly System.Text.RegularExpressions.Regex _compiledRegex = new(@"^(?<year>\d{4})\-(?<month>\d{1,2})( \[(?<quarter>\d{1,2})\])?$", RegexOptions.Compiled);
#endif

#if NET7_0
    /// <summary>
    /// Parses the input string by using a pre-compiled `Regex`.
    /// </summary>
    [Benchmark]
    public void RegexPrecompiled()
    {
        const string input = "2008-8 [2]";
        (int year, int month, int? quarter) = OldRegex(input, GetRegex());

        if ((year != 2008) || (month != 8) || (quarter != 2))
        {
            throw new Exception();
        }
    }

    [GeneratedRegex(@"^(?<year>\d{4})\-(?<month>\d{1,2})( \[(?<quarter>\d{1,2})\])?$")]
    private static partial System.Text.RegularExpressions.Regex GetRegex();

    /// <summary>
    /// Parses the input string by using a non-compiled `Regex` on `ReadOnlySpan&lt;char&gt;`.
    /// </summary>
    [Benchmark]
    public void RegexNotCompiledSpan()
    {
        const string input = "2008-8 [2]";
        (int year, int month, int? quarter) = RegexSpan(input, _notCompiledRegex);

        if ((year != 2008) || (month != 8) || (quarter != 2))
        {
            throw new Exception();
        }
    }

    private static (int year, int month, int? quarter) RegexSpan(string input, System.Text.RegularExpressions.Regex regex)
    {
        Match match = regex.Match(input);
        if (!match.Success)
        {
            throw new ArgumentException("Invalid input", nameof(input));
        }

        ReadOnlySpan<char> yearAsString = match.Groups["year"].ValueSpan;
        int year = int.Parse(yearAsString);

        ReadOnlySpan<char> monthAsString = match.Groups["month"].ValueSpan;
        int month = int.Parse(monthAsString);

        int? quarter;
        ReadOnlySpan<char> quarterAsString = match.Groups["quarter"].ValueSpan;
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
    /// Parses the input string by using a compiled `Regex` on `ReadOnlySpan&lt;char&gt;`.
    /// </summary>
    [Benchmark]
    public void RegexCompiledSpan()
    {
        const string input = "2008-8 [2]";
        (int year, int month, int? quarter) = RegexSpan(input, _compiledRegex);

        if ((year != 2008) || (month != 8) || (quarter != 2))
        {
            throw new Exception();
        }
    }

    /// <summary>
    /// Parses the input string by using a pre-compiled `Regex` on `ReadOnlySpan&lt;char&gt;`.
    /// </summary>
    [Benchmark]
    public void RegexPreCompiledSpan()
    {
        const string input = "2008-8 [2]";
        (int year, int month, int? quarter) = RegexSpan(input, GetRegex());

        if ((year != 2008) || (month != 8) || (quarter != 2))
        {
            throw new Exception();
        }
    }
#endif
}