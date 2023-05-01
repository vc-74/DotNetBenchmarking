namespace DotNetBenchmarking.FunctionCall;

/// <summary>
/// Prototype for functions taking two integers in parameter and returning an integer.
/// </summary>
public delegate int TakesTwoIntsReturnsInt(int a, int b);

/// <summary>
/// Prototype for functions taking one integer in parameter and returning an integer.
/// </summary>
public delegate int TakesOneIntReturnsInt(int b);

/// <summary>
/// Prototype for functions taking a integer in parameter and not returning anything.
/// </summary>
public delegate void TakesAnInt(int x);
