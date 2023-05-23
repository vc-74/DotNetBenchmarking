using System;

namespace DotNetBenchmarking.Regex.UnitTests;

/// <summary>
/// <see cref="Regex"/> tests.
/// </summary>
public class RegexTests
{
    private delegate (int year, int month, int? quarter) Parser(string input);

    [Fact]
    public void Manual() => TestParser(Regex.Manual);

    private static void TestParser(Parser parser)
    {
        TestValidInput("2002-8", parser,
            expectedYear: 2002, expectedMonth: 8, expectedQuarter: null);

        TestValidInput("2002-08", parser,
            expectedYear: 2002, expectedMonth: 8, expectedQuarter: null);

        TestValidInput("2002-8 [3]", parser,
            expectedYear: 2002, expectedMonth: 8, expectedQuarter: 3);

        TestValidInput("2002-08 [03]", parser,
            expectedYear: 2002, expectedMonth: 8, expectedQuarter: 3);

        TestInvalidInput("Invalid", parser);
    }

    private static void TestValidInput(string input, Parser parser,
        int expectedYear, int expectedMonth, int? expectedQuarter)
    {
        (int year, int month, int? quarter) = parser(input);

        Assert.Equal(expectedYear, year);
        Assert.Equal(expectedMonth, month);
        Assert.Equal(expectedQuarter, quarter);
    }

    private static void TestInvalidInput(string input, Parser parser)
    {
        ArgumentException argumentException = Assert.Throws<ArgumentException>(() => parser(input));
        Assert.Equal("input", argumentException.ParamName);
    }

    [Fact]
    public void IndexOfString() => TestParser(Regex.IndexOfString);

    [Fact]
    public void IndexOfSpan() => TestParser(Regex70.IndexOfSpan);

    [Fact]
    public void RegexNotCompiledString() => TestParser(Regex.RegexNotCompiledString);

    [Fact]
    public void RegexCompiledString() => TestParser(Regex.RegexCompiledString);

    [Fact]
    public void RegexPreCompiledString() => TestParser(Regex70.RegexPreCompiledString);

    [Fact]
    public void RegexNotCompiledSpan() => TestParser(Regex70.RegexNotCompiledSpan);

    [Fact]
    public void RegexCompiledSpan() => TestParser(Regex70.RegexCompiledSpan);

    [Fact]
    public void RegexPreCompiledSpan() => TestParser(Regex70.RegexPreCompiledSpan);
}