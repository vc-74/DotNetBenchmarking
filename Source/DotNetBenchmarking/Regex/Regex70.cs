using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace DotNetBenchmarking.Regex;

#if NET7_0
/// <summary>
/// Compares different methods of building and executing a regular expression available on .net7.
/// </summary>
[SimpleJob(RuntimeMoniker.Net70)]
[MemoryDiagnoser]
public partial class Regex70
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
    /// Parses the input string by using `IndexOf` on `ReadOnlySpan&lt;char&gt;`.
    /// </summary>
    [Benchmark]
    public void IndexOfSpan()
    {
        for (int i = 0; i < Loops; i++)
        {
            IndexOfSpan("2002-8 [2]");
        }
    }

    internal static (int year, int month, int? quarter) IndexOfSpan(string input)
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

#pragma warning disable SYSLIB1045 // Convert to 'GeneratedRegexAttribute'.
    private static readonly System.Text.RegularExpressions.Regex _notCompiledRegex = new(@"^(?<year>\d{4})\-(?<month>\d{1,2})( \[(?<quarter>\d{1,2})\])?$");
#pragma warning restore SYSLIB1045 // Convert to 'GeneratedRegexAttribute'.

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

#pragma warning disable SYSLIB1045 // Convert to 'GeneratedRegexAttribute'.
    private static readonly System.Text.RegularExpressions.Regex _compiledRegex = new(@"^(?<year>\d{4})\-(?<month>\d{1,2})( \[(?<quarter>\d{1,2})\])?$", RegexOptions.Compiled);
#pragma warning restore SYSLIB1045 // Convert to 'GeneratedRegexAttribute'.

    /// <summary>
    /// Parses the input string by using a pre-compiled `Regex` and extracts the groups as strings.
    /// </summary>
    [Benchmark]
    public void RegexPrecompiledString()
    {
        for (int i = 0; i < Loops; i++)
        {
            RegexPreCompiledString("2002-8 [2]");
        }
    }

    internal static (int year, int month, int? quarter) RegexPreCompiledString(string input) => RegexString(input, GetRegex());

    [GeneratedRegex(@"^(?<year>\d{4})\-(?<month>\d{1,2})( \[(?<quarter>\d{1,2})\])?$")]
    private static partial System.Text.RegularExpressions.Regex GetRegex();

    /// <summary>
    /// Parses the input string by using a non-compiled `Regex` on `ReadOnlySpan&lt;char&gt;` and extracts the groups as <see cref="ReadOnlySpan{Char}"/>.
    /// </summary>
    [Benchmark]
    public void RegexNotCompiledSpan()
    {
        for (int i = 0; i < Loops; i++)
        {
            RegexNotCompiledSpan("2002-8 [2]");
        }
    }

    internal static (int year, int month, int? quarter) RegexNotCompiledSpan(string input) => RegexSpan(input, _notCompiledRegex);

    internal static (int year, int month, int? quarter) RegexSpan(string input, System.Text.RegularExpressions.Regex regex)
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
        for (int i = 0; i < Loops; i++)
        {
            RegexCompiledSpan("2002-8 [2]");
        }
    }

    internal static (int year, int month, int? quarter) RegexCompiledSpan(string input) => RegexSpan(input, _compiledRegex);

    /// <summary>
    /// Parses the input string by using a pre-compiled `Regex` on `ReadOnlySpan&lt;char&gt;`.
    /// </summary>
    [Benchmark]
    public void RegexPreCompiledSpan()
    {
        for (int i = 0; i < Loops; i++)
        {
            RegexPreCompiledSpan("2002-8 [2]");
        }
    }

    internal static (int year, int month, int? quarter) RegexPreCompiledSpan(string input) => RegexSpan(input, GetRegex());
}
#endif
