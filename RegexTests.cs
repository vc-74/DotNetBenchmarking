using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;

namespace DotNetBenchmarking;

/// <summary>
/// Compares different methods of building and executing a regular expression.
/// </summary>
[MemoryDiagnoser]
public partial class RegexTests
{
    [Benchmark(Baseline = true)]
    public void Manual()
    {
        const string input = "2008-8 [2]";
        Manual(input, out int year, out int month, out int? cybm);

        if ((year != 2008) || (month != 8) || (cybm != 2))
        {
            throw new Exception();
        }
    }

    private static void Manual(string input, out int year, out int month, out int? cybm)
    {
        bool inYear = true, inMonth = false, inCybm = false;
        year = 0; month = 0; cybm = 0;

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
                                    inCybm = true;
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
        else if (inCybm)
        {
            cybm = value;
        }
        else
        {
            invalidInput = true;
        }

        if (invalidInput)
        {
            throw new ArgumentException("Invalid input", nameof(input));
        }
    }

    [Benchmark]
    public void IndexOfString()
    {
        const string input = "2008-8 [2]";
        IndexOfString(input, out int year, out int month, out int? cybm);

        if ((year != 2008) || (month != 8) || (cybm != 2))
        {
            throw new Exception();
        }
    }

    private static void IndexOfString(string input, out int year, out int month, out int? cybm)
    {
        int hyphenPosition = input.IndexOf('-');
        if (hyphenPosition == -1)
        {
            throw new ArgumentException("Invalid input", nameof(input));
        }

        string yearPart = input.Substring(0, hyphenPosition);
        if (!int.TryParse(yearPart, out year))
        {
            throw new ArgumentException("Invalid input", nameof(input));
        }

        int cybmPosition = input.IndexOf(" [", hyphenPosition);
        if (cybmPosition == -1)
        {
            string monthPart = input.Substring(hyphenPosition + 1);
            if (!int.TryParse(monthPart, out month))
            {
                throw new ArgumentException("Invalid input", nameof(input));
            }

            cybm = null;
        }
        else
        {
            if (!input.EndsWith("]"))
            {
                throw new ArgumentException("Invalid input", nameof(input));
            }

            string monthPart = input.Substring(hyphenPosition + 1, cybmPosition - hyphenPosition - 1);
            if (!int.TryParse(monthPart, out month))
            {
                throw new ArgumentException("Invalid input", nameof(input));
            }

            string cybmPart = input.Substring(cybmPosition + 2, input.Length - cybmPosition - 3);
            if (!int.TryParse(cybmPart, out int cybmValue))
            {
                throw new ArgumentException("Invalid input", nameof(input));
            }
            cybm = cybmValue;
        }
    }

#if NET7_0
    [Benchmark]
    public void IndexOfRanges()
    {
        const string input = "2008-8 [2]";
        IndexOfRanges(input, out int year, out int month, out int? cybm);

        if ((year != 2008) || (month != 8) || (cybm != 2))
        {
            throw new Exception();
        }
    }

    private static void IndexOfRanges(string input, out int year, out int month, out int? cybm)
    {
        int hyphenPosition = input.IndexOf('-');
        if (hyphenPosition == -1)
        {
            throw new ArgumentException("Invalid input", nameof(input));
        }

        string yearPart = input[..hyphenPosition];
        if (!int.TryParse(yearPart, out year))
        {
            throw new ArgumentException("Invalid input", nameof(input));
        }

        int cybmPosition = input.IndexOf(" [", hyphenPosition);
        if (cybmPosition == -1)
        {
            string monthPart = input[(hyphenPosition + 2)..];
            if (!int.TryParse(monthPart, out month))
            {
                throw new ArgumentException("Invalid input", nameof(input));
            }

            cybm = null;
        }
        else
        {
            if (!input.EndsWith("]"))
            {
                throw new ArgumentException("Invalid input", nameof(input));
            }

            string monthPart = input[(hyphenPosition + 1)..(cybmPosition + 1)];
            if (!int.TryParse(monthPart, out month))
            {
                throw new ArgumentException("Invalid input", nameof(input));
            }

            string cybmPart = input[(cybmPosition + 2)..(input.Length - 1)];
            if (!int.TryParse(cybmPart, out int cybmValue))
            {
                throw new ArgumentException("Invalid input", nameof(input));
            }
            cybm = cybmValue;
        }
    }

    [Benchmark]
    public void IndexOfSpans()
    {
        const string input = "2008-8 [2]";
        IndexOfSpans(input, out int year, out int month, out int? cybm);

        if ((year != 2008) || (month != 8) || (cybm != 2))
        {
            throw new Exception();
        }
    }

    private static void IndexOfSpans(string input, out int year, out int month, out int? cybm)
    {
        ReadOnlySpan<char> currentSpan = input;

        int hyphenPosition = currentSpan.IndexOf('-');
        if (hyphenPosition == -1)
        {
            throw new ArgumentException("Invalid input", nameof(input));
        }

        ReadOnlySpan<char> yearPart = currentSpan[..hyphenPosition];
        if (!int.TryParse(yearPart, out year))
        {
            throw new ArgumentException("Invalid input", nameof(input));
        }

        currentSpan = currentSpan[(hyphenPosition + 1)..];

        int cybmPosition = currentSpan.IndexOf(" [");
        if (cybmPosition == -1)
        {
            if (!int.TryParse(currentSpan, out month))
            {
                throw new ArgumentException("Invalid input", nameof(input));
            }

            cybm = null;
        }
        else
        {
            if (!currentSpan.EndsWith("]"))
            {
                throw new ArgumentException("Invalid input", nameof(input));
            }

            ReadOnlySpan<char> monthPart = currentSpan[0..cybmPosition];
            if (!int.TryParse(monthPart, out month))
            {
                throw new ArgumentException("Invalid input", nameof(input));
            }

            ReadOnlySpan<char> cybmPart = currentSpan[(cybmPosition + 2)..^1];
            if (!int.TryParse(cybmPart, out int cybmValue))
            {
                throw new ArgumentException("Invalid input", nameof(input));
            }
            cybm = cybmValue;
        }
    }
#endif

    [Benchmark]
    public void RegexNotCompiled()
    {
        const string input = "2008-8 [2]";
        OldRegex(input, _notCompiledRegex, out int year, out int month, out int? cybm);

        if ((year != 2008) || (month != 8) || (cybm != 2))
        {
            throw new Exception();
        }
    }
    private static readonly Regex _notCompiledRegex = new(@"^(?<year>\d{4})\-(?<month>\d{1,2})( \[(?<cybm>\d{1,2})\])?$");

    private static void OldRegex(string input, Regex regex, out int year, out int month, out int? cybm)
    {
        Match match = regex.Match(input);
        if (!match.Success)
        {
            throw new ArgumentException("Invalid input", nameof(input));
        }

        string yearAsString = match.Groups["year"].Value;
        year = int.Parse(yearAsString);

        string monthAsString = match.Groups["month"].Value;
        month = int.Parse(monthAsString);

        string cybmAsString = match.Groups["cybm"].Value;
        if (int.TryParse(cybmAsString, out int cybmValue))
        {
            cybm = cybmValue;
        }
        else
        {
            cybm = null;
        }
    }

    [Benchmark]
    public void RegexCompiled()
    {
        const string input = "2008-8 [2]";
        OldRegex(input, _compiledRegex, out int year, out int month, out int? cybm);

        if ((year != 2008) || (month != 8) || (cybm != 2))
        {
            throw new Exception();
        }
    }
    private static readonly Regex _compiledRegex = new(@"^(?<year>\d{4})\-(?<month>\d{1,2})( \[(?<cybm>\d{1,2})\])?$", RegexOptions.Compiled);


#if NET7_0
    [Benchmark]
    public void RegexPrecompiled()
    {
        const string input = "2008-8 [2]";
        OldRegex(input, GetRegex(), out int year, out int month, out int? cybm);

        if ((year != 2008) || (month != 8) || (cybm != 2))
        {
            throw new Exception();
        }
    }

    [GeneratedRegex(@"^(?<year>\d{4})\-(?<month>\d{1,2})( \[(?<cybm>\d{1,2})\])?$")]
    private static partial Regex GetRegex();

    [Benchmark]
    public void RegexNotCompiledSpan()
    {
        const string input = "2008-8 [2]";
        RegexSpan(input, _notCompiledRegex, out int year, out int month, out int? cybm);

        if ((year != 2008) || (month != 8) || (cybm != 2))
        {
            throw new Exception();
        }
    }

    private static void RegexSpan(string input, Regex regex, out int year, out int month, out int? cybm)
    {
        Match match = regex.Match(input);
        if (!match.Success)
        {
            throw new ArgumentException("Invalid input", nameof(input));
        }

        ReadOnlySpan<char> yearAsString = match.Groups["year"].ValueSpan;
        year = int.Parse(yearAsString);

        ReadOnlySpan<char> monthAsString = match.Groups["month"].ValueSpan;
        month = int.Parse(monthAsString);

        ReadOnlySpan<char> cybmAsString = match.Groups["cybm"].ValueSpan;
        if (int.TryParse(cybmAsString, out int cybmValue))
        {
            cybm = cybmValue;
        }
        else
        {
            cybm = null;
        }
    }

    [Benchmark]
    public void RegexCompiledSpan()
    {
        const string input = "2008-8 [2]";
        RegexSpan(input, _compiledRegex, out int year, out int month, out int? cybm);

        if ((year != 2008) || (month != 8) || (cybm != 2))
        {
            throw new Exception();
        }
    }

    [Benchmark]
    public void RegexPreCompiledSpan()
    {
        const string input = "2008-8 [2]";
        RegexSpan(input, GetRegex(), out int year, out int month, out int? cybm);

        if ((year != 2008) || (month != 8) || (cybm != 2))
        {
            throw new Exception();
        }
    }
#endif
}