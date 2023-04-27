namespace DotNetBenchmarking.FunctionCall;

/// <summary>
/// Method of producing a dynamically built delegate.
/// </summary>
public enum CompilationType
{
    /// <summary>
    /// Build a <see cref="System.Reflection.Emit.DynamicMethod"/> by emitting IL code.
    /// </summary>
    DynamicFunction,

    /// <summary>
    /// Build a new Adder type with an Add static method by emitting IL code.
    /// </summary>
    TypeBuilder,

    /// <summary>
    /// Create an expression tree and compile it to a lambda.
    /// </summary>
    ExpressionTree
}
